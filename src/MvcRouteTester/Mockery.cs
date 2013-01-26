using System;
using System.Collections.Specialized;
using System.Web;
using Moq;

namespace MvcRouteTester
{
	public class Mockery
	{
		public static HttpContextBase ContextForUrl(string url)
		{
			var routeParts = url.Split('?');
			var relativeUrl = routeParts[0];
			var queryParams = UrlHelpers.MakeQueryParams(url);

			var httpContextMock = new Mock<HttpContextBase>();
			httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(relativeUrl);
			httpContextMock.Setup(c => c.Request.QueryString).Returns(queryParams);
			httpContextMock.Setup(c => c.Request.Params).Returns(queryParams);
			httpContextMock.Setup(c => c.Request.PathInfo).Returns(string.Empty);
			return httpContextMock.Object;
		}
	}
}
