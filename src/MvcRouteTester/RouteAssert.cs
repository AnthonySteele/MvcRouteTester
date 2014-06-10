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
			HasRoute(routes, url, string.Empty, BodyFormat.None, expectations);
		}

		public static void HasRoute(RouteCollection routes, string url, string body, BodyFormat bodyFormat, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedRouteValues = propertyReader.RouteValues(expectations);
			WebRouteAssert.HasRoute(routes, HttpMethod.Get, url, body, bodyFormat, expectedRouteValues);
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
			var expectedRouteValues = new RouteValues(expectedProps);
			WebRouteAssert.HasRoute(routes, HttpMethod.Get, url, string.Empty, BodyFormat.None, expectedRouteValues);
		}

		public static void HasRoute(RouteCollection routes, string url, string body, IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			WebRouteAssert.HasRoute(routes, HttpMethod.Get, url, body, BodyFormat.FormUrl, expectedRouteValues);
		}

		public static void HasRoute(RouteCollection routes, string url, string body, BodyFormat bodyFormat, IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			WebRouteAssert.HasRoute(routes, HttpMethod.Get, url, body, bodyFormat, expectedRouteValues);
		}

		public static void HasRoute(RouteCollection routes, string url, HttpMethod httpMethod, IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			WebRouteAssert.HasRoute(routes, httpMethod, url, string.Empty, BodyFormat.None, expectedRouteValues);
		}

		public static void HasRoute(RouteCollection routes, string url, HttpMethod httpMethod, string body, BodyFormat bodyFormat, IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			WebRouteAssert.HasRoute(routes, httpMethod, url, body, bodyFormat, expectedRouteValues);
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

		public static void GeneratesActionUrl(RouteCollection routes,
			string expectedUrl, object fromProps,
			HttpMethod httpMethod = null, string appPath = "/", string requestBody = null)
		{
			if (httpMethod == null)
			{
				httpMethod = HttpMethod.Get;
			}

			var propertyReader = new PropertyReader();
			var expectedRouteValues = propertyReader.RouteValues(fromProps);

			WebRouteAssert.GeneratesActionUrl(routes, httpMethod, expectedUrl, expectedRouteValues, appPath, requestBody);
		}

		public static void GeneratesActionUrl(RouteCollection routes, string expectedUrl, IDictionary<string, string> fromProps, string appPath = "/")
		{
			var expectedRouteValues = new RouteValues(fromProps);
			WebRouteAssert.GeneratesActionUrl(routes, HttpMethod.Get, expectedUrl, expectedRouteValues, appPath, null);
		}

		public static void GeneratesActionUrl(RouteCollection routes, string expectedUrl, string action, string controller,
			HttpMethod httpMethod = null, string appPath = null, string requestBody = null)
		{
			if (httpMethod == null)
			{
				httpMethod = HttpMethod.Get;
			}

			WebRouteAssert.GeneratesActionUrl(routes, httpMethod, expectedUrl, action, controller, 
				appPath, new RouteValueDictionary(), requestBody);
		}

		public static void GeneratesActionUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl,
			 IDictionary<string, string> fromProps, string appPath = null, string requestBody = null)
		{
			var fromRouteValues = new RouteValues(fromProps);
			WebRouteAssert.GeneratesActionUrl(routes, httpMethod, expectedUrl, fromRouteValues, appPath, requestBody);
		}

		public static void GeneratesActionUrl(RouteCollection routes, string expectedUrl, string action,
			HttpMethod httpMethod = null, string appPath = null, string requestBody = null)
		{
			if (httpMethod == null)
			{
				httpMethod = HttpMethod.Get;
			}

			WebRouteAssert.GeneratesActionUrl(routes, httpMethod, expectedUrl, action, null, appPath, new RouteValueDictionary(), requestBody);
		}

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method
		/// </summary>
	    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod)
	    {
	        HasApiRoute(config, url, httpMethod, null);
	    }

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method
		/// </summary>
		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers)
		{
			ApiRouteAssert.HasRoute(config, url, httpMethod, headers);
		}

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method and meets the expectations
		/// </summary>
	    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, object expectations)
	    {
            HasApiRoute(config, url, httpMethod, (Dictionary<string, string>)null, expectations);
	    }

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method and meets the expectations
		/// </summary>
		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers,  object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.RouteValues(expectations);

			ApiRouteAssert.HasRoute(config, url, httpMethod, headers, string.Empty, BodyFormat.None, expectedProps);
		}

	    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod,
	        string body, object expectations)
	    {
            HasApiRoute(config, url, httpMethod, null, body, expectations);
	    }

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, string body, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.RouteValues(expectations);

			ApiRouteAssert.HasRoute(config, url, httpMethod, headers, body, BodyFormat.FormUrl, expectedProps);
		}

	    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod,
	        string body, BodyFormat bodyFormat, object expectations)
	    {
            HasApiRoute(config, url, httpMethod, null, body, bodyFormat, expectations);
	    }

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, string body, BodyFormat bodyFormat, object expectations)
		{
			var propertyReader = new PropertyReader();
			var expectedProps = propertyReader.RouteValues(expectations);

			ApiRouteAssert.HasRoute(config, url, httpMethod, headers, body, bodyFormat, expectedProps);
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

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			ApiRouteAssert.HasRoute(config, url, httpMethod, headers, string.Empty, BodyFormat.None, expectedRouteValues);
		}

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, string body, IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			ApiRouteAssert.HasRoute(config, url, httpMethod, headers, body, BodyFormat.FormUrl, expectedRouteValues);
		}

		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, Dictionary<string, string> headers, string body, BodyFormat bodyFormat,  IDictionary<string, string> expectedProps)
		{
			var expectedRouteValues = new RouteValues(expectedProps);
			ApiRouteAssert.HasRoute(config, url, httpMethod, headers, body, bodyFormat, expectedRouteValues);
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

		public static void AddIgnoreAttributes(IEnumerable<Type> types)
		{
			PropertyReader.AddIgnoreAttributes(types);
		}

		public static void AddIgnoreAttribute(Type type)
		{
			PropertyReader.AddIgnoreAttribute(type);
		}

		public static void ClearIgnoreAttributes()
		{
			PropertyReader.ClearIgnoreAttributes();
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
