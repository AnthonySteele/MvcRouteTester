using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	class TestController : Controller
	{
		public ActionResult Index(string foo, string bar)
		{
			return new EmptyResult();
		}
	}
}
