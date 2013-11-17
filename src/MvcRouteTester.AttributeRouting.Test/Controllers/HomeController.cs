using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class HomeController : Controller
	{
		[Route("home/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}
}
