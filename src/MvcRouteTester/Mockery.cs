using System.Collections.Specialized;
using System.Web;

namespace MvcRouteTester
{
	public class Mockery
	{
		public static HttpContextBase ContextForUrl(string url)
		{
			var routeParts = url.Split('?');
			var relativeUrl = routeParts[0];
			var queryParams = UrlHelpers.MakeQueryParams(url);

			return new PrivateHttpContext(new PrivateHttpRequest(relativeUrl, queryParams));
		}

		class PrivateHttpContext : HttpContextBase
		{
			readonly HttpRequestBase request;

			public PrivateHttpContext(HttpRequestBase request)
			{
				this.request = request;
			}

			public override HttpRequestBase Request {
				get {
					return this.request;
				}
			}
		}

		class PrivateHttpRequest : HttpRequestBase
		{
			readonly string relativeUrl;
			readonly NameValueCollection queryParams;

			public PrivateHttpRequest(string relativeUrl, NameValueCollection queryParams)
			{
				this.relativeUrl = relativeUrl;
				this.queryParams = queryParams;
			}

			public override string AppRelativeCurrentExecutionFilePath {
				get {
					return this.relativeUrl;
				}
			}
			public override NameValueCollection QueryString {
				get {
					return this.queryParams;
				}
			}

			public override NameValueCollection Params {
				get {
					return this.queryParams;
				}
			}

			public override string PathInfo {
				get {
					return string.Empty;
				}
			}
		}
	}
}