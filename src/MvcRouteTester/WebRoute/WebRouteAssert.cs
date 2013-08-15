using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
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
	        var requestContext = RequestContext(httpMethod, requestBody, appPath);

	        var routeValueDictionary = new RouteValueDictionary();
	        var generatedUrl = UrlHelper.GenerateUrl(null, action, controller, routeValueDictionary, routes, requestContext,
	            true);
            
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
	        var urlHelper = new UrlHelper(requestContext, routes);
	        return urlHelper;
	    }

	    private static RequestContext RequestContext(HttpMethod httpMethod, string requestBody, string appPath)
	    {
	        var httpContext = HttpMockery.ContextForUrl(httpMethod, appPath, requestBody);
	        var requestContext = new RequestContext(httpContext, new RouteData());
	        return requestContext;
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
	            var message =
	                string.Format("Generated url does not equal to expected url. Generated: '{0}' | Expected: '{1}'",
	                    generatedUrl, expectedUrl);
	            Asserts.Fail(message);
	        }
	    }

	    private static RouteValueDictionary BuildRouteValueDictionary(object routeValues)
        {
            PropertyInfo[] infos = routeValues.GetType().GetProperties();
            var routeValueDictionary = new RouteValueDictionary();

            foreach (PropertyInfo info in infos)
            {
                routeValueDictionary.Add(info.Name, info.GetValue(routeValues, null));
            }

            return routeValueDictionary;
        }
    }
}
