using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class GetPostController : Controller
	{
        [Route("GetPost/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}

        [HttpPost]
        [Route("GetPost/index/{id}")]
		public ActionResult Index(int id)
		{
			return new EmptyResult();
		}
	}
}
