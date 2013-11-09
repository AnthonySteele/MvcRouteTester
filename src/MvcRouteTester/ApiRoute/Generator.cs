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
using MvcRouteTester.Common;

namespace MvcRouteTester.ApiRoute
{
	/// <summary>
	/// code from http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/
	/// </summary>
	internal class Generator
	{
		readonly HttpConfiguration config;
		readonly HttpRequestMessage request;

		IHttpRouteData matchedRoute;
		IHttpControllerSelector controllerSelector;
		HttpControllerContext controllerContext;

		public Generator(HttpConfiguration conf, HttpRequestMessage req)
		{
			config = conf;
			request = req;
			
			GenerateRouteData();
		}

		public bool HasMatchedRoute
		{
			get { return matchedRoute != null; }
		}

		public bool HasHandler<THander>()
		{
			if (matchedRoute.Route.Handler == null)
			{
				return false;
			}

			return matchedRoute.Route.Handler.GetType() == typeof(THander);
		}

		public RouteValues ReadRequestProperties(string url, HttpMethod httpMethod, BodyFormat bodyFormat)
		{
			if (! CheckValid(url, httpMethod))
			{
				return new RouteValues();
			}

			var actualProps = new RouteValues();
			actualProps.Controller = ControllerName();
			actualProps.Action = ActionName();

			actualProps.AddRange(GetRouteParams());

			var queryParams = UrlHelpers.ReadQueryParams(url);
			actualProps.AddRange(queryParams);
			actualProps.AddRange(ReadPropertiesFromBodyContent(bodyFormat));


			return actualProps;
		}

		private bool CheckValid(string url, HttpMethod httpMethod)
		{
			if (!HasMatchedRoute)
			{
				var noRouteDataMessage = string.Format("No route matched url '{0}'", url);
				Asserts.Fail(noRouteDataMessage);
				return false;
			}

			if (!IsControllerRouteFound())
			{
				var routeNotFoundMessage = string.Format("Route with controller not found for url '{0}'", url);
				Asserts.Fail(routeNotFoundMessage);
				return false;
			}

			if (!IsMethodAllowed())
			{
				var methodNotAllowedMessage = string.Format("Method {0} is not allowed on url '{1}'", httpMethod, url);
				Asserts.Fail(methodNotAllowedMessage);
				return false;
			}

			return true;
		}

		public IList<RouteValue> GetRouteParams()
		{
			var actionDescriptor = MakeActionDescriptor();
			var actionParams = actionDescriptor.GetParameters();

			var result = new List<RouteValue>();
			var routeDataValues = GetRouteData();
			if (routeDataValues != null)
			{
				foreach (var param in actionParams)
				{
					var values = ProcessActionParam(param, routeDataValues);
					result.AddRange(values);
				}
			}

			return result;
		}

		private static IList<RouteValue> ProcessActionParam(HttpParameterDescriptor param, HttpRouteData routeDataValues)
		{
			var propertyReader = new PropertyReader();

			if (propertyReader.IsSimpleType(param.ParameterType))
			{
				return ProcessSimpleActionParam(param, routeDataValues);
			}

			return ProcessCompoundActionParam(param, routeDataValues, propertyReader);
		}

		private static IList<RouteValue> ProcessSimpleActionParam(HttpParameterDescriptor param, HttpRouteData routeDataValues)
		{
			var routeValues = new List<RouteValue>();

			var paramName = param.ParameterName;
			var value = ReadParamWithRouteValue(paramName, routeDataValues.Values);
			if (value != null)
			{
				routeValues.Add(value);
			}
			return routeValues;
		}

		private static IList<RouteValue> ProcessCompoundActionParam(HttpParameterDescriptor param, HttpRouteData routeDataValues, PropertyReader propertyReader)
		{
			var routeValues = new List<RouteValue>();

			var fieldNames = propertyReader.SimplePropertyNames(param.ParameterType);
			foreach (var fieldName in fieldNames)
			{
				var value = ReadParamWithRouteValue(fieldName.ToLowerInvariant(), routeDataValues.Values);
				if (value != null)
				{
					routeValues.Add(value);
				}
			}

			return routeValues;
		}

		private static RouteValue ReadParamWithRouteValue(string paramName, IDictionary<string, object> values)
		{
			if (values.ContainsKey(paramName))
			{
				var paramValue = values[paramName];
				if (paramValue != null)
				{
					return new RouteValue(paramName, paramValue, RouteValueOrigin.Params);
				}
			}

			return null;
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
				return;
			}

			if (!IsControllerRouteFound())
			{
				var routeNotFoundMessage = string.Format("Route with controller not found for url '{0}'", url);
				Asserts.Fail(routeNotFoundMessage);
				return;
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
				controllerSelector = (IHttpControllerSelector) Activator.CreateInstance(ApiRouteAssert.ControllerSelectorType, config);
				controllerContext = new HttpControllerContext(config, matchedRoute, request);
			}
		}

		private HttpActionDescriptor MakeActionDescriptor()
		{
			var actionSelector = new ApiControllerActionSelector();
			return actionSelector.SelectAction(controllerContext);
		}

		private IList<RouteValue> ReadPropertiesFromBodyContent(BodyFormat bodyFormat)
		{
			var bodyTask = request.Content.ReadAsStringAsync();
			var body = bodyTask.Result;

			var bodyReader = new BodyReader();
			return bodyReader.ReadBody(body, bodyFormat);
		}
	}
}
