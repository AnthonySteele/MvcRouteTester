using System.Web.Mvc;

namespace MvcRouteTester.Test.Areas.SomeArea
{
	public class TestController: Controller
	{
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}
}
