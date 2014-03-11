using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	[RouteArea("FooArea")]
	public class WithAreaController : Controller
	{
		[Route("Index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}
}
