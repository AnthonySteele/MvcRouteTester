using System.Net.Http;
using System.Web;
using System.Web.Routing;

namespace MvcRouteTester.HttpMocking
{
	public class HttpMockery
	{
		public static HttpContextBase ContextForUrl(string url)
		{
            return ContextForUrl(HttpMethod.Get, url);
		}

        public static HttpContextBase ContextForUrl(HttpMethod method, string url)
		{
			var routeParts = url.Split('?');
			var relativeUrl = routeParts[0];
			var queryParams = UrlHelpers.MakeQueryParams(url);


            var request = new MockHttpRequest(method, relativeUrl, queryParams);
            var httpContext = new MockHttpContext(request);

            var requestContext = new RequestContext(httpContext, new RouteData());
            request.SetRequestContext(requestContext);

            return httpContext;
		}
	}
}