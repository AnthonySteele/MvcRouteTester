using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using System.Web.Routing;
using MvcRouteTester.Common;

namespace MvcRouteTester.HttpMocking
{
	public class HttpMockery
	{
		public static HttpContextBase ContextForUrl(string url)
		{
			return ContextForUrl(HttpMethod.Get, url, string.Empty);
		}

		public static HttpContextBase ContextForUrl(HttpMethod method, string url, string requestBody)
		{
			if (string.IsNullOrEmpty(url))
			{
				url = string.Empty;
			}
			var routeParts = url.Split('?');
			var relativeUrl = routeParts[0];
			var queryParamsValues = UrlHelpers.ReadQueryParams(url);
			var queryParams = AsNameValueCollection(queryParamsValues);

			var request = new MockHttpRequest(method, relativeUrl, queryParams, requestBody);
			var response = new MockHttpResponse();
			var httpContext = new MockHttpContext(request, response);

			var requestContext = new RequestContext(httpContext, new RouteData());
			request.SetContext(requestContext);

			return httpContext;
		}

		private static NameValueCollection AsNameValueCollection(IList<RouteValue> queryParamsValues)
		{
			var result = new NameValueCollection();
			foreach (var routeValue in queryParamsValues)
			{
				result.Add(routeValue.Name, routeValue.ValueAsString);
			}
			return result;
		}
	}
}