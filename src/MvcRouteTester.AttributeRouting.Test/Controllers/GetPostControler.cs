using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class GetPostController : Controller
	{
		[GET("GetPost/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}

		[POST("GetPost/index/{id}")]
		public ActionResult Index(int id)
		{
			return new EmptyResult();
		}
	}
}
