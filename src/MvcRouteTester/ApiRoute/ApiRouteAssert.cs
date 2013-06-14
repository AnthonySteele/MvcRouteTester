using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using MvcRouteTester.Assertions;
using MvcRouteTester.Common;

namespace MvcRouteTester.ApiRoute
{
	internal static class ApiRouteAssert
	{
		static ApiRouteAssert()
		{
			ControllerSelectorType = typeof (DefaultHttpControllerSelector);
		}

		internal static Type ControllerSelectorType;

		internal static void HasRoute(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			ReadRequestProperties(config, absoluteUrl, httpMethod, string.Empty);
		}

		internal static void HasRoute(HttpConfiguration config, string url, HttpMethod httpMethod, string body, IDictionary<string, string> expectedProps)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var actualProps = ReadRequestProperties(config, absoluteUrl, httpMethod, body);

			var verifier = new Verifier();
			verifier.VerifyExpectations(expectedProps, actualProps, url);
		}

		internal static void NoRoute(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var routeGenerator = new Generator(config, request);

			if (routeGenerator.IsControllerRouteFound())
			{
				var hasRouteMessage = string.Format("Found a route for url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		internal static void RouteDoesNotHaveMethod(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(httpMethod, absoluteUrl);
			var routeGenerator = new Generator(config, request);

			routeGenerator.CheckNoMethod(url, httpMethod);
		}

		public static void RouteDoesNotHaveMethod(HttpConfiguration config, string url, Type controllerType, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(httpMethod, absoluteUrl);
			var routeGenerator = new Generator(config, request);

			routeGenerator.CheckControllerHasNoMethod(url, httpMethod, controllerType);
		}

		internal static void RouteMatches(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var routeGenerator = new Generator(config, request);

			if (!routeGenerator.HasMatchedRoute)
			{
				var hasRouteMessage = string.Format("Did not match a route for url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		internal static void NoRouteMatches(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var routeGenerator = new Generator(config, request);

			if (routeGenerator.HasMatchedRoute)
			{
				var hasRouteMessage = string.Format("Matched a route for url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		private static IDictionary<string, string> ReadRequestProperties(HttpConfiguration config, string url, HttpMethod httpMethod, string body)
		{
			var request = new HttpRequestMessage(httpMethod, url);
			request.Content = new StringContent(body);

			var routeGenerator = new Generator(config, request);
			return routeGenerator.ReadRequestProperties(url, httpMethod);
		}
	}
}
