using System;
using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class RouteParamsCasesController : Controller
	{
		public ActionResult TwoIntAction(int id1, int id2)
		{
			return new EmptyResult();
		}

		public ActionResult GuidAction(Guid id)
		{
			return new EmptyResult();
		}
	}
}
