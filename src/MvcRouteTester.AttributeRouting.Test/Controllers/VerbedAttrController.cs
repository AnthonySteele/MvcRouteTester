using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class VerbedAttrController : Controller
	{
		[HttpGet]
		[Route("VerbedAttr")]
		public ActionResult Get()
		{
			return new EmptyResult();
		}

		[HttpPost]
		[Route("VerbedAttr")]
		public ActionResult Post()
		{
			return new EmptyResult();
		}
	}
}
