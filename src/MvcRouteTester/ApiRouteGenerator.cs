using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

using MvcRouteTester.Assertions;

namespace MvcRouteTester
{
	/// <summary>
	/// code from http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/
	/// </summary>
	internal class ApiRouteGenerator
	{
		readonly HttpConfiguration config;
		readonly HttpRequestMessage request;

		IHttpRouteData matchedRoute;
		IHttpControllerSelector controllerSelector;
		HttpControllerContext controllerContext;

		public ApiRouteGenerator(HttpConfiguration conf, HttpRequestMessage req)
		{
			config = conf;
			request = req;
			
			GenerateRouteData();
		}

		public bool HasMatchedRoute
		{
			get { return matchedRoute != null; }
		}

		public Dictionary<string, string> ReadRouteProperties(string url, HttpMethod httpMethod)
		{
			if (!HasMatchedRoute)
			{
				var noRouteDataMessage = string.Format("No route matched url '{0}'", url);
				Asserts.Fail(noRouteDataMessage);
			}

			if (!IsControllerRouteFound())
			{
				var routeNotFoundMessage = string.Format("Route with controller not found for url '{0}'", url);
				Asserts.Fail(routeNotFoundMessage);
			}

			if (!IsMethodAllowed())
			{
				var methodNotAllowedMessage = string.Format("Method {0} is not allowed on url '{1}'", httpMethod, url);
				Asserts.Fail(methodNotAllowedMessage);
			}

			var actualProps = new Dictionary<string, string>
				{
					{ "controller", ControllerName() },
					{ "action", ActionName() }
				};

			var routeParams = GetRouteParams();

			foreach (var paramKey in routeParams.Keys)
			{
				actualProps.Add(paramKey, routeParams[paramKey]);
			}

			var queryParams = UrlHelpers.MakeQueryParams(url);
			foreach (var key in queryParams.AllKeys)
			{
				if (actualProps.ContainsKey(key))
				{
					actualProps[key] = queryParams[key];
				}
				else
				{
					actualProps.Add(key, queryParams[key]);
				}
			}

			return actualProps;
		}

		public IDictionary<string, string> GetRouteParams()
		{
			var actionDescriptor = MakeActionDescriptor();
			var actionParams = actionDescriptor.GetParameters();

			var result = new Dictionary<string, string>();
			var routeDataValues = GetRouteData();
			if (routeDataValues != null)
			{
				foreach (var param in actionParams)
				{
					ProcessActionParam(param, routeDataValues, result);
				}
			}

			return result;
		}

		private static void ProcessActionParam(HttpParameterDescriptor param, HttpRouteData routeDataValues, Dictionary<string, string> result)
		{
			var propertyReader = new PropertyReader();

			if (propertyReader.IsSimpleType(param.ParameterType))
			{
				var paramName = param.ParameterName;
				AddParamWithRouteValue(paramName, routeDataValues.Values, result);
			}
			else
			{
				var fieldNames = propertyReader.SimplePropertyNames(param.ParameterType);
				foreach (var fieldName in fieldNames)
				{
					AddParamWithRouteValue(fieldName.ToLowerInvariant(), routeDataValues.Values, result);
				}
			}
		}

		private static void AddParamWithRouteValue(string paramName, IDictionary<string, object> values, Dictionary<string, string> result)
		{
			if (values.ContainsKey(paramName))
			{
				var paramValue = values[paramName];
				if (paramValue != null)
				{
					result.Add(paramName, paramValue.ToString());
				}
			}
		}

		private HttpRouteData GetRouteData()
		{
			if (! request.Properties.Any(prop => prop.Value is HttpRouteData))
			{
				return null;
			}

			var routeDataProp = request.Properties.First(prop => prop.Value is HttpRouteData);
			return routeDataProp.Value as HttpRouteData;
		}

		public string ActionName()
		{
			if (controllerContext.ControllerDescriptor == null)
			{
				ControllerType();
			}

			var descriptor = MakeActionDescriptor();

			return descriptor.ActionName;
		}

		public bool IsControllerRouteFound()
		{
			if (! HasMatchedRoute)
			{
				return false;
			}

			try
			{
				return !string.IsNullOrEmpty(ActionName());
			}
			catch (HttpResponseException hrex)
			{
				var status = hrex.Response.StatusCode;
				return status != HttpStatusCode.NotFound;
			}
		}

		public bool IsMethodAllowed()
		{
			try
			{
				return ! string.IsNullOrEmpty(ActionName());
			}
			catch (HttpResponseException hrex)
			{
				var status = hrex.Response.StatusCode;
				return status != HttpStatusCode.MethodNotAllowed;
			}
		}

		public Type ControllerType()
		{
			var descriptor = controllerSelector.SelectController(request);
			controllerContext.ControllerDescriptor = descriptor;
			return descriptor.ControllerType;
		}

		public string ControllerName()
		{
			var controllerType = ControllerType();
			var name = controllerType.Name;
			if (name.EndsWith("Controller"))
			{
				name = name.Substring(0, name.Length - 10);
			}

			return name;
		}

		public void CheckNoMethod(string url, HttpMethod httpMethod)
		{
			if (! HasMatchedRoute)
			{
				var noRouteDataMessage = string.Format("No route matched url '{0}'", url);
				Asserts.Fail(noRouteDataMessage);
			}

			if (!IsControllerRouteFound())
			{
				var routeNotFoundMessage = string.Format("Route with controller not found for url '{0}'", url);
				Asserts.Fail(routeNotFoundMessage);
			}

			if (IsMethodAllowed())
			{
				var methodAllowedMessage = string.Format("Method {0} is allowed on url '{1}'", httpMethod, url);
				Asserts.Fail(methodAllowedMessage);
			}
		}

		public void CheckControllerHasNoMethod(string url, HttpMethod httpMethod, Type controllerType)
		{
			CheckNoMethod(url, httpMethod);
			var actualControllerType = ControllerType();

			if (controllerType != actualControllerType)
			{
				var methodAllowedMessage = string.Format("Expected controller {0}, but goes to {1} for url '{2}'", 
					controllerType.Name, actualControllerType.Name, url);
				Asserts.Fail(methodAllowedMessage);
			}
		}

		private void GenerateRouteData()
		{
			matchedRoute = config.Routes.GetRouteData(request);

			if (matchedRoute != null)
			{
				request.Properties[HttpPropertyKeys.HttpRouteDataKey] = matchedRoute;
				controllerSelector = new DefaultHttpControllerSelector(config);
				controllerContext = new HttpControllerContext(config, matchedRoute, request);
			}
		}

		private HttpActionDescriptor MakeActionDescriptor()
		{
			var actionSelector = new ApiControllerActionSelector();
			return actionSelector.SelectAction(controllerContext);
		}
	}
}
