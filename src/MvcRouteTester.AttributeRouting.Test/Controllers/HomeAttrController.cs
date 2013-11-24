using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class HomeAttrController : Controller
	{
		[Route("homeattr/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}
}
