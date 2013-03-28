using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace MvcRouteTester.HttpMocking
{
	internal class MockHttpContext : HttpContextBase
	{
		private readonly HttpRequestBase request;
		private readonly Dictionary<object, object> items = new Dictionary<object, object>();

		public MockHttpContext(HttpRequestBase request)
		{
			this.request = request;
		}

		public override HttpRequestBase Request
		{
			get { return request; }
		}

		public override IDictionary Items
		{
			get { return items; }
		}
	}
}