using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace MvcRouteTester
{
	/// <summary>
	/// Asserts on MVC routes and API routes
	/// Adapted from Phil Haack's route test code for MVC Routes
	/// see http://haacked.com/archive/2007/12/16/testing-routes-in-asp.net-mvc.aspx
	/// and Filip W's code for testing Api routes
	/// See http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/
	/// </summary>
	public class RouteAssert
	{
		/// <summary>
		/// Asserts that the route exists 
		/// </summary>
		public static void HasRoute(RouteCollection routes, string url)
		{
			var webRouteReader = new WebRouteReader();
			var routeData = webRouteReader.GetRouteDataForUrl(routes, url);

			var message = string.Format("Should have found the route to '{0}'", url);
			Asserts.NotNull(routeData, message);
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
			var webRouteReader = new WebRouteReader();
			var routeData = webRouteReader.GetRouteDataForUrl(routes, url);

			var message = string.Format("Should have found the route to '{0}'", url);
			Asserts.NotNull(routeData, message);

			var actualProps = webRouteReader.GetRouteProperties(routeData);
			var verifier = new Verifier();
			verifier.VerifyExpectations(expectedProps, actualProps, url);
		}

		/// <summary>
		/// Asserts that the route does not exist
		/// </summary>
		public static void NoRoute(RouteCollection routes, string url)
		{
			var httpContext = Mockery.ContextForUrl(url);
			var routeData = routes.GetRouteData(httpContext);

			var message = string.Format("Should not have found the route to '{0}'", url);
			Asserts.Null(routeData, message);
		}

		/// <summary>
		/// Asserts that the API route exists, has the specified Http method
		/// </summary>
		public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			ReadApiRouteProperties(config, absoluteUrl, httpMethod);
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
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var actualProps = ReadApiRouteProperties(config, absoluteUrl, httpMethod);

			var verifier = new Verifier();
			verifier.VerifyExpectations(expectedProps, actualProps, url);
		}

		private static IDictionary<string, string> ReadApiRouteProperties(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var request = new HttpRequestMessage(httpMethod, url);
			var apiRouteGenerator = new ApiRouteGenerator(config, request);
			return apiRouteGenerator.ReadRouteProperties(url, httpMethod);
		}

		/// <summary>
		/// Asserts that the API route does not exist
		/// </summary>
		public static void NoApiRoute(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var apiRouteGenerator = new ApiRouteGenerator(config, request);

			if (apiRouteGenerator.RouteData != null)
			{
				var hasRouteMessage = string.Format("Found route to url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		/// <summary>
		/// Asserts that the API route exists but does not have the specified Http method
		/// </summary>
		public static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(httpMethod, absoluteUrl);
			var apiRouteGenerator = new ApiRouteGenerator(config, request);

			if (apiRouteGenerator.RouteData == null)
			{
				var noRouteMessage = string.Format("No route to url '{0}'", url);
				Asserts.Fail(noRouteMessage);
			}

			if (apiRouteGenerator.IsMethodAllowed())
			{
				var methodAllowedMessage = string.Format("Method {0} is allowed on url '{1}'", httpMethod, url);
				Asserts.Fail(methodAllowedMessage);
			}
		}
	}
}
