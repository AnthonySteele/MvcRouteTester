using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	public class WithNullableController : ApiController
	{
		public HttpResponseMessage Get(int? id)
		{
			return new HttpResponseMessage();
		}
	}
}
