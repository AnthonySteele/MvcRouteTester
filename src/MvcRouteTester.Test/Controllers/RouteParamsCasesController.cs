using System;
using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class RouteParamsCasesController : Controller
	{
		public ActionResult GuidAction(Guid id)
		{
			return new EmptyResult();
		}

		public ActionResult BoolAction(bool id)
		{
			return new EmptyResult();
		}
	}
}
