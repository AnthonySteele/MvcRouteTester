using System.Web;

namespace MvcRouteTester.HttpMocking
{
	public class MockHttpResponse : HttpResponseBase
	{
		public override string ApplyAppPathModifier(string virtualPath)
		{
			return virtualPath;
		}
	}
}
