using System;
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

		internal static void HasRoute(RouteCollection routes, HttpMethod method, string url, string body, RouteValues expectedProps)
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
			var verifier = new Verifier(expectedProps, actualProps, url);
			verifier.VerifyExpectations();
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

		internal static void GeneratesActionUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl, 
			RouteValues fromProps, string appPath, string requestBody)
		{
			fromProps.CheckDataOk();
			if (!fromProps.DataOk)
			{
				return;
			}

			var routeValueDict = fromProps.AsRouteValueDictionary();

			GeneratesActionUrl(routes, httpMethod, expectedUrl,
				fromProps.Action, fromProps.Controller, appPath, routeValueDict, requestBody);
		}


		internal static void GeneratesActionUrl(RouteCollection routes, HttpMethod httpMethod, string expectedUrl, string action, string controller, 
			string appPath, RouteValueDictionary routeValueDictionary, string requestBody)
		{
			var urlHelper = GetUrlHelper(routes, httpMethod, appPath, requestBody);
			var generatedUrl = urlHelper.Action(action, controller, routeValueDictionary);

			AssertGeneratedUrlExpectedUrl(expectedUrl, generatedUrl);
		}

		private static UrlHelper GetUrlHelper(RouteCollection routes, HttpMethod httpMethod, string appPath, string requestBody)
		{
			var requestContext = RequestContext(httpMethod, appPath, requestBody);
			return new UrlHelper(requestContext, routes);
		}

		private static RequestContext RequestContext(HttpMethod httpMethod, string appPath, string requestBody)
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
