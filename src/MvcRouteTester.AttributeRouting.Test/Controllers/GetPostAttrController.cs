using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class GetPostController : Controller
	{
		[HttpGet]
		[Route("GetPostAttr/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}

		[HttpPost]
		[Route("GetPostAttr/index/{id}")]
		public ActionResult Index(int id)
		{
			return new EmptyResult();
		}
	}
}
