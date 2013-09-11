using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace MvcRouteTester.HttpMocking
{
	internal class MockHttpContext : HttpContextBase
	{
		private readonly HttpRequestBase request;
		private readonly HttpResponseBase response;
		private readonly Dictionary<object, object> items = new Dictionary<object, object>();

		public MockHttpContext(HttpRequestBase request)
		{
			this.request = request;
		}
		public MockHttpContext(HttpRequestBase request, HttpResponseBase response) : this(request)
		{
			this.response = response;
		}

		public override HttpRequestBase Request
		{
			get { return request; }
		}

		public override HttpResponseBase Response
		{
			get
			{
				return response;
			}
		}

		public override IDictionary Items
		{
			get { return items; }
		}

		public override void RewritePath(string filePath, string pathInfo, string queryString)
		{
		}

		public override void RewritePath(string path)
		{
		}
	}
}