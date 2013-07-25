using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class WithOptionalParamController : Controller
	{
		public ActionResult Search(string country, string action, string id)
		{
			return new EmptyResult();
		}
	}
}
