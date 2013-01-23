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
			var routeParts = url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
			var relativeUrl = routeParts[0];
			string paramsString = string.Empty;
			if (routeParts.Length > 1)
			{
				paramsString = routeParts[1];
			}

			var queryParams = MakeQueryParams(paramsString);

			var httpContextMock = new Mock<HttpContextBase>();
			httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(relativeUrl);
			httpContextMock.Setup(c => c.Request.QueryString).Returns(queryParams);
			httpContextMock.Setup(c => c.Request.Params).Returns(queryParams);
			httpContextMock.Setup(c => c.Request.PathInfo).Returns(string.Empty);
			return httpContextMock.Object;
		}

		private static NameValueCollection MakeQueryParams(string paramsString)
		{
			var queryParameters = new NameValueCollection();

			if (! string.IsNullOrWhiteSpace(paramsString))
			{
				var paramsWithValues = paramsString.Split('&');
				foreach (var paramWithValue in paramsWithValues)
				{
					var nameValuePair = paramWithValue.Split('=');
					string value = string.Empty;
					if (nameValuePair.Length > 1)
					{
						value = nameValuePair[1];
					}

					queryParameters.Add(nameValuePair[0], value);
				}
			}

			return queryParameters;
		}
	}
}
