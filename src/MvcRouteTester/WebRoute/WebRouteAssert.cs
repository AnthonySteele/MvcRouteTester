using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRouteTester.Assertions;
using MvcRouteTester.Common;
using MvcRouteTester.HttpMocking;

namespace MvcRouteTester.WebRoute
{
	internal static class WebRouteAssert
	{

		internal static void HasRoute(RouteCollection routes, string url)
		{
			var pathUrl = UrlHelpers.PrependTilde(url);

			var httpContext = HttpMockery.ContextForUrl(pathUrl);
			var routeData = routes.GetRouteData(httpContext);

			if (routeData == null)
			{
				var message = string.Format("Should have found the route to '{0}'", url);
				Asserts.Fail(message);
			}
		}

		internal static void HasRoute(RouteCollection routes, HttpMethod method, string url, string body, IDictionary<string, string> expectedProps)
		{
			var pathUrl = UrlHelpers.PrependTilde(url);
			var httpContext = HttpMockery.ContextForUrl(method, pathUrl, body);
			var routeData = routes.GetRouteData(httpContext);

			if (routeData == null)
			{
				var message = string.Format("Should have found the route to '{0}'", url);
				Asserts.Fail(message);
			}

			var webRouteReader = new Reader();
			var actualProps = webRouteReader.GetRequestProperties(routeData, httpContext.Request);
			var verifier = new Verifier();
			verifier.VerifyExpectations(expectedProps, actualProps, url);
		}

		internal static void NoRoute(RouteCollection routes, string url)
		{
			var pathUrl = UrlHelpers.PrependTilde(url);
			var httpContext = HttpMockery.ContextForUrl(pathUrl);
			var routeData = routes.GetRouteData(httpContext);

			if (routeData != null)
			{
				var message = string.Format("Should not have found the route to '{0}'", url);
				Asserts.Fail(message);
			}
		}

		internal static void IsIgnoredRoute(RouteCollection routes, string url)
		{
			var pathUrl = UrlHelpers.PrependTilde(url);
			var httpContext = HttpMockery.ContextForUrl(pathUrl);
			var routeData = routes.GetRouteData(httpContext);

			if (routeData == null)
			{
				var message = string.Format("Should have found the route to '{0}'", url);
				Asserts.Fail(message);
				return;
			}

			var isIgnored = (routeData.RouteHandler is StopRoutingHandler);
			if (!isIgnored)
			{
				var message = string.Format("Route to '{0}' is not ignored", url);
				Asserts.Fail(message);
			}
		}

		internal static void IsNotIgnoredRoute(RouteCollection routes, string url)
		{
			var pathUrl = UrlHelpers.PrependTilde(url);
			var httpContext = HttpMockery.ContextForUrl(pathUrl);
			var routeData = routes.GetRouteData(httpContext);

			if (routeData == null)
			{
				var message = string.Format("Should have found the route to '{0}'", url);
				Asserts.Fail(message);
				return;
			}

			var isIgnored = (routeData.RouteHandler is StopRoutingHandler);
			if (isIgnored)
			{
				var message = string.Format("Route to '{0}' is ignored", url);
				Asserts.Fail(message);
			}
		}

		internal static void GeneratesUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl, string requestBody,
			string action, string controller, string appPath)
		{
			var routeValueDictionary = new RouteValueDictionary();

			GeneratesUrl(routes, httpMethod, expectedUrl, requestBody, action, controller, appPath, routeValueDictionary);
		}

		internal static void GeneratesUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl, string requestBody,
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

			GeneratesUrl(routes, httpMethod, expectedUrl, requestBody, action, controller, appPath, routeValueDictionary);
		}

		internal static void GeneratesUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl, string requestBody,
			string action, string controller, string appPath, RouteValueDictionary routeValueDictionary)
		{
			var requestContext = RequestContext(httpMethod, requestBody, appPath);

			var generatedUrl = UrlHelper.GenerateUrl(null, 
				action, controller, routeValueDictionary, routes, requestContext, true);

			AssertGeneratedUrlExpectedUrl(expectedUrl, generatedUrl);
		}

		internal static void GeneratesActionUrl(RouteCollection routes, 
			HttpMethod httpMethod, string requestBody, string appPath, 
			string expectedUrl, string action)
		{
			var urlHelper = GetUrlHelper(routes, httpMethod, requestBody, appPath);

			var generatedUrl = urlHelper.Action(action);

			AssertGeneratedUrlExpectedUrl(expectedUrl, generatedUrl);
		}

		internal static void GeneratesActionUrl(RouteCollection routes, 
			HttpMethod httpMethod, string requestBody, string appPath,
			string expectedUrl, string action, string controller)
		{
			var urlHelper = GetUrlHelper(routes, httpMethod, requestBody, appPath);

			var generatedUrl = urlHelper.Action(action, controller);

			AssertGeneratedUrlExpectedUrl(expectedUrl, generatedUrl);
		}

		private static UrlHelper GetUrlHelper(RouteCollection routes, HttpMethod httpMethod, string requestBody,
			string appPath)
		{
			var requestContext = RequestContext(httpMethod, requestBody, appPath);
			return new UrlHelper(requestContext, routes);
		}

		private static RequestContext RequestContext(HttpMethod httpMethod, string requestBody, string appPath)
		{
			var httpContext = HttpMockery.ContextForUrl(httpMethod, appPath, requestBody);
			return new RequestContext(httpContext, new RouteData());
		}

		private static void AssertGeneratedUrlExpectedUrl(string expectedUrl, string generatedUrl)
		{
			if (string.IsNullOrWhiteSpace(generatedUrl))
			{
				var message = string.Format("Generated Url is null or empty");
				Asserts.Fail(message);
				return;
			}

			if (!generatedUrl.Equals(expectedUrl, StringComparison.InvariantCultureIgnoreCase))
			{
				var message = string.Format(
					"Generated url does not equal to expected url. Generated: '{0}', expected: '{1}'",
					generatedUrl, expectedUrl);
				Asserts.Fail(message);
			}
		}
	}
}
