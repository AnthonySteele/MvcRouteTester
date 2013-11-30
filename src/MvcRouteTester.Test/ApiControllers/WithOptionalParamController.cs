using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	public class WithOptionalParamController : ApiController
	{
		public HttpResponseMessage Get(int paramA, int? paramB =null)
		{
			return new HttpResponseMessage();
		}
	}
}
