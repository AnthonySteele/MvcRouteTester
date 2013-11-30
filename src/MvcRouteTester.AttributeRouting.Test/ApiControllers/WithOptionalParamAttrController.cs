using System.Net.Http;
using System.Web.Http;

using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.ApiControllers
{
	[RoutePrefix("someapi")]
	public class WithOptionalParamAttrController : ApiController
	{
		[Route("withoptionalparam/{paramA}")]
		public HttpResponseMessage Get(int paramA, int? paramB = null)
		{
			return new HttpResponseMessage();
		}
	}
}
