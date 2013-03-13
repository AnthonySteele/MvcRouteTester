using System.Web;

namespace MvcRouteTester.HttpMocking
{
	internal class MockHttpContext : HttpContextBase
	{
		private readonly HttpRequestBase request;

		public MockHttpContext(HttpRequestBase request)
		{
			this.request = request;
		}

		public override HttpRequestBase Request 
		{
			get { return request; }
		}
	}
}