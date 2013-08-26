using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class TwoRouteParamsController : Controller
	{
		public ActionResult TwoIntAction(int id1, int id2)
		{
			return new EmptyResult();
		}

		public ActionResult TwoStringAction(string id1, string id2)
		{
			return new EmptyResult();
		}
	}
}