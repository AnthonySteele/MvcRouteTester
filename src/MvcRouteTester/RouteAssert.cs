using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using MvcRouteTester.ApiRoute;
using MvcRouteTester.Assertions;
using MvcRouteTester.Common;
using MvcRouteTester.WebRoute;

namespace MvcRouteTester
{
	/// <summary>
	/// Asserts on MVC routes and API routes
	/// Adapted from Phil Haack's route test code for MVC Routes
	/// see http://haacked.com/archive/2007/12/16/testing-routes-in-asp.net-mvc.aspx
	/// and Filip W's code for testing Api routes
	/// See http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/
	/// </summary>
	public static class RouteAssert
	{

		/// <summary>
		/// Asserts that the route exists 
		/// </summary>
		public static void HasRoute(RouteCollection routes, string url)
		{
			WebRouteAssert.HasRoute(routes, url);
		}

		/// <summary>
		/// Asserts that the route exists and meets expectations
		/// </summary>
		public static void HasRoute(RouteCollection routes, string url, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.Properties(expectations);
			HasRoute(routes, url, string.Empty, expectedProps);
		}

		public static void HasRoute(RouteCollection routes, string url, string body, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.Properties(expectations);
			HasRoute(routes, url, body, expectedProps);
		}

		public static void HasRoute(RouteCollection routes, string url, string controller, string action)
		{
			var expectedProps = new Dictionary<string, string>
				{
					{ "controller", controller },
					{ "action" , action }
				};

			HasRoute(routes, url, string.Empty, expectedProps);
		}

		public static void HasRoute(RouteCollection routes, string url, IDictionary<string, string> expectedProps)
		{
			WebRouteAssert.HasRoute(routes, HttpMethod.Get, url, string.Empty, expectedProps);
		}

		public static void HasRoute(RouteCollection routes, string url, string body, IDictionary<string, string> expectedProps)
		{
			WebRouteAssert.HasRoute(routes, HttpMethod.Get, url, body, expectedProps);
		}

		/// <summary>
		/// Asserts that the route does not exist
		/// </summary>
		public static void NoRoute(RouteCollection routes, string url)
		{
			WebRouteAssert.NoRoute(routes, url);
		}

		/// <summary>
		/// Assert that the route is ignored
		/// </summary>
		/// <param name="routes"></param>
		/// <param name="url"></param>
		public static void IsIgnoredRoute(RouteCollection routes, string url)
		{
			WebRouteAssert.IsIgnoredRoute(routes, url);
		}

		/// <summary>
		/// Assert that the route is not ignored
		/// </summary>
		/// <param name="routes"></param>
		/// <param name="url"></param>
		public static void IsNotIgnoredRoute(RouteCollection routes, string url)
		{
			WebRouteAssert.IsNotIgnoredRoute(routes, url);
		}

		public static void GeneratesUrl(RouteCollection routes, string expectedUrl, string action, string controller, string currentUrl = "/")
		{
			WebRouteAssert.GeneratesUrl(routes, HttpMethod.Get, expectedUrl, null, action, controller, currentUrl);
		}

		public static void GeneratesUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl, string requestBody,
			 IDictionary<string, string> fromProps, string appPath = "/")
		{
			if (!fromProps.ContainsKey("controller"))
			{
				var message = string.Format("No controller property found in fromProps");
				Asserts.Fail(message);
				return;
			}
			if (!fromProps.ContainsKey("action"))
			{
				var message = string.Format("No action property found in fromProps");
				Asserts.Fail(message);
				return;
			}
			string controller = null, action = null;
			var routeValueDictionary = new RouteValueDictionary();

			foreach (var fromProp in fromProps)
			{
				switch (fromProp.Key)
				{
					case "controller":
						controller = fromProp.Value;
						break;
					case "action":
						action = fromProp.Value;
						break;
					default:
						routeValueDictionary.Add(fromProp.Key, fromProp.Value);
						break;
				}
			}

			WebRouteAssert.GeneratesUrl(routes, httpMethod, expectedUrl, requestBody, action, controller, appPath, routeValueDictionary);
		}

		public static void GeneratesActionUrl(RouteCollection routes,
			string expectedUrl, string action,
			HttpMethod httpMethod = null, string requestBody = null, string appPath = "/")
		{
			if (httpMethod == null) httpMethod = HttpMethod.Get;
			WebRouteAssert.GeneratesActionUrl(routes, httpMethod, requestBody, appPath, expectedUrl, action);
		}


		public static void GeneratesActionUrl(RouteCollection routes,
			string expectedUrl, string action, string controller,
			HttpMethod httpMethod = null, string requestBody = null, string appPath = "/")
		{
			if (httpMethod == null)
			{
				httpMethod = HttpMethod.Get;
			}

			WebRouteAssert.GeneratesActionUrl(routes, httpMethod, requestBody, appPath, expectedUrl, action, controller);
		}


		/// <summary>
		/// Asserts that the API route exists, has the specified Http method
		/// </summary>
		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			ApiRouteAssert.HasRoute(config, url, httpMethod);
		}

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method and meets the expectations
		/// </summary>
		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.Properties(expectations);

			HasApiRoute(config, url, httpMethod, expectedProps);
		}

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, string body, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.Properties(expectations);

			HasApiRoute(config, url, httpMethod, body, expectedProps);
		}

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, string controller, string action)
		{
			var expectedProps = new Dictionary<string, string>
				{
					{ "controller", controller },
					{ "action" , action }
				};

			HasApiRoute(config, url, httpMethod, expectedProps);
		}

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, IDictionary<string, string> expectedProps)
		{
			ApiRouteAssert.HasRoute(config, url, httpMethod, string.Empty, expectedProps);
		}

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, string body, IDictionary<string, string> expectedProps)
		{
			ApiRouteAssert.HasRoute(config, url, httpMethod, body, expectedProps);
		}

		/// <summary>
		/// Asserts that the API route does not exist
		/// </summary>
		public static void NoApiRoute(HttpConfiguration config, string url)
		{
			ApiRouteAssert.NoRoute(config, url);
		}

		/// <summary>
		/// Asserts that an API route for the url exists but does not have the specified Http method.
		/// </summary>
		public static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			ApiRouteAssert.RouteDoesNotHaveMethod(config, url, httpMethod);
		}

		/// <summary>
		/// Asserts that the API route for the url exists to the specified controller, but does not have the specified Http method.
		/// </summary>
		public static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, Type controllerType, HttpMethod httpMethod)
		{
			ApiRouteAssert.RouteDoesNotHaveMethod(config, url, controllerType, httpMethod);
		}

		/// <summary>
		/// Asserts that an entry in the routing table is found that matches this url
		/// </summary>
		public static void ApiRouteMatches(HttpConfiguration config, string url)
		{
			ApiRouteAssert.RouteMatches(config, url);
		}

		/// <summary>
		/// Asserts that an entry in the routing table is not found that matches this url
		/// </summary>
		public static void NoApiRouteMatches(HttpConfiguration config, string url)
		{
			ApiRouteAssert.NoRouteMatches(config, url);
		}

		public static void UseAssertEngine(IAssertEngine engine)
		{
			Asserts.AssertEngine = engine;
		}

		/// <summary>
		/// Sets the type of selector to be used to locate the controller for api routes
		/// </summary>
		public static void UseHttpControllerSelector(Type selector)
		{
			ApiRouteAssert.ControllerSelectorType = selector;
		}
	}
}
