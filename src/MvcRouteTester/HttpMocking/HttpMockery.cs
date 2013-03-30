using System.Net.Http;
using System.Web;

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

            return new MockHttpContext(new MockHttpRequest(method, relativeUrl, queryParams));
		}
	}
}