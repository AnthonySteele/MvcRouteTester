using System.Web;

namespace MvcRouteTester.HttpMocking
{
	public class HttpMockery
	{
		public static HttpContextBase ContextForUrl(string url)
		{
			var routeParts = url.Split('?');
			var relativeUrl = routeParts[0];
			var queryParams = UrlHelpers.MakeQueryParams(url);

			return new MockHttpContext(new MockHttpRequest(relativeUrl, queryParams));
		}
	}
}