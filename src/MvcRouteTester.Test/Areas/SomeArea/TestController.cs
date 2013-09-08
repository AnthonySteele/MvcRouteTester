using System.Web.Mvc;

namespace MvcRouteTester.Test.Areas.SomeArea
{
	public class TestController: Controller
	{
		public ActionResult Index()
		{
			return new EmptyResult();
		}
		public ActionResult Index(int id)
		{
			return new EmptyResult();
		}

		public ActionResult About()
		{
			return new EmptyResult();
		}
	}
}
