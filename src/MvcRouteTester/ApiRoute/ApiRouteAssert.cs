using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using MvcRouteTester.Assertions;
using MvcRouteTester.Common;

namespace MvcRouteTester.ApiRoute
{
	internal static class ApiRouteAssert
	{
		internal static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			ReadApiRouteProperties(config, absoluteUrl, httpMethod);
		}

		internal static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, IDictionary<string, string> expectedProps)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var actualProps = ReadApiRouteProperties(config, absoluteUrl, httpMethod);

			var verifier = new Verifier();
			verifier.VerifyExpectations(expectedProps, actualProps, url);
		}

		internal static void NoApiRoute(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var apiRouteGenerator = new Generator(config, request);

			if (apiRouteGenerator.IsControllerRouteFound())
			{
				var hasRouteMessage = string.Format("Found route to url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		internal static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(httpMethod, absoluteUrl);
			var apiRouteGenerator = new Generator(config, request);

			apiRouteGenerator.CheckNoMethod(url, httpMethod);
		}

		public static void ApiRouteDoesNotHaveMethod(HttpConfiguration config, string url, Type controllerType, HttpMethod httpMethod)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(httpMethod, absoluteUrl);
			var apiRouteGenerator = new Generator(config, request);

			apiRouteGenerator.CheckControllerHasNoMethod(url, httpMethod, controllerType);
		}

		internal static void ApiRouteMatches(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var apiRouteGenerator = new Generator(config, request);

			if (!apiRouteGenerator.HasMatchedRoute)
			{
				var hasRouteMessage = string.Format("Did not match a route for url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		internal static void NoApiRouteMatches(HttpConfiguration config, string url)
		{
			var absoluteUrl = UrlHelpers.MakeAbsolute(url);
			var request = new HttpRequestMessage(HttpMethod.Get, absoluteUrl);
			var apiRouteGenerator = new Generator(config, request);

			if (apiRouteGenerator.HasMatchedRoute)
			{
				var hasRouteMessage = string.Format("Matched a route for url '{0}'", url);
				Asserts.Fail(hasRouteMessage);
			}
		}

		private static IDictionary<string, string> ReadApiRouteProperties(HttpConfiguration config, string url, HttpMethod httpMethod)
		{
			var request = new HttpRequestMessage(httpMethod, url);
			var apiRouteGenerator = new Generator(config, request);
			return apiRouteGenerator.ReadRouteProperties(url, httpMethod);
		}
	}
}
