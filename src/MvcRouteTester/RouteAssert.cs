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
			HasRoute(routes, url, expectedProps);
		}

		public static void HasRoute(RouteCollection routes, string url, string controller, string action)
		{
			var expectedProps = new Dictionary<string, string>
				{
					{ "controller", controller },
					{ "action" , action }
				};

			HasRoute(routes, url, expectedProps);
		}

		public static void HasRoute(RouteCollection routes, string url, IDictionary<string, string> expectedProps)
		{
			WebRouteAssert.HasRoute(routes, url, expectedProps);
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

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method
		/// </summary>
		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			ApiRouteAssert.HasApiRoute(config, url, httpMethod);
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
			ApiRouteAssert.HasApiRoute(config, url, httpMethod, expectedProps);
		}

		/// <summary>
		/// Asserts that the API route does not exist
		/// </summary>
		public static void NoApiRoute(HttpConfiguration config, string url)
		{
			ApiRouteAssert.NoApiRoute(config, url);
		}

		/// <summary>
		/// Asserts that an API route for the url exists but does not have the specified Http method.
		/// </summary>
		public static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			ApiRouteAssert.ApiRouteDoesNotHaveMethod(config, url, httpMethod);
		}

		/// <summary>
		/// Asserts that the API route for the url exists to the specified controller, but does not have the specified Http method.
		/// </summary>
		public static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, Type controllerType, HttpMethod httpMethod)
		{
			ApiRouteAssert.ApiRouteDoesNotHaveMethod(config, url, controllerType, httpMethod);
		}

		/// <summary>
		/// Asserts that an entry in the routing table is found that matches this url
		/// </summary>
		public static void ApiRouteMatches(HttpConfiguration config, string url)
		{
			ApiRouteAssert.ApiRouteMatches(config, url);
		}

		/// <summary>
		/// Asserts that an entry in the routing table is not found that matches this url
		/// </summary>
		public static void NoApiRouteMatches(HttpConfiguration config, string url)
		{
			ApiRouteAssert.NoApiRouteMatches(config, url);
		}

		public static void UseAssertEngine(IAssertEngine engine)
		{
			Asserts.AssertEngine = engine;
		}
	}
}
