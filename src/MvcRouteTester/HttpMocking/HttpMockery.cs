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
			var queryParams = UrlHelpers.MakeQueryParams(url);

			var request = new MockHttpRequest(method, relativeUrl, queryParams, requestBody);
			var response = new MockHttpResponse();
			var httpContext = new MockHttpContext(request, response);

			var requestContext = new RequestContext(httpContext, new RouteData());
			request.SetContext(requestContext);

			return httpContext;
		}
	}
}