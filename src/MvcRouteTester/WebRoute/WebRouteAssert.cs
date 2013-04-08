using System.Collections.Generic;
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

		internal static void HasRoute(RouteCollection routes, string url, IDictionary<string, string> expectedProps)
		{
			var pathUrl = UrlHelpers.PrependTilde(url);
			var httpContext = HttpMockery.ContextForUrl(pathUrl);
			var routeData = routes.GetRouteData(httpContext);

			if (routeData == null)
			{
				var message = string.Format("Should have found the route to '{0}'", url);
				Asserts.Fail(message);
			}

			var webRouteReader = new Reader();
			var actualProps = webRouteReader.GetRouteProperties(routeData, httpContext.Request.Params);
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
	}
}
