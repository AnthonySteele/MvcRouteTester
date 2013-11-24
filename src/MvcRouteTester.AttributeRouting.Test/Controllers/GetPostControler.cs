using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class GetPostAttrController : Controller
	{
		[GET("GetPostAttr/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}

		[POST("GetPostAttr/index/{id}")]
		public ActionResult Index(int id)
		{
			return new EmptyResult();
		}
	}
}
