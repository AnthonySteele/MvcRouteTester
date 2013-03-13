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

		internal class PrivateHttpContext : HttpContextBase
		{
			private readonly HttpRequestBase request;

			public PrivateHttpContext(HttpRequestBase request)
			{
				this.request = request;
			}

			public override HttpRequestBase Request 
			{
				get { return request; }
			}
		}

		internal class PrivateHttpRequest : HttpRequestBase
		{
			readonly string relativeUrl;
			readonly NameValueCollection queryParams;

			public PrivateHttpRequest(string relativeUrl, NameValueCollection queryParams)
			{
				this.relativeUrl = relativeUrl;
				this.queryParams = queryParams;
			}

			public override string AppRelativeCurrentExecutionFilePath
			{
				get { return relativeUrl; }
			}

			public override NameValueCollection QueryString 
			{
				get { return queryParams; }
			}

			public override NameValueCollection Params 
			{
				get { return queryParams; }
			}

			public override string PathInfo 
			{
				get { return string.Empty; }
			}
		}
	}
}