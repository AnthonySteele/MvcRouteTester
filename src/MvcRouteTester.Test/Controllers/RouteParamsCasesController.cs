using System;
using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public struct InputModelStruct
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}
	
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

		public ActionResult StructAction(InputModelStruct inputModel)
		{
			return new EmptyResult();
		}
	}
}
