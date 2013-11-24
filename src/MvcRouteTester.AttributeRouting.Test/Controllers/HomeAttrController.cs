using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class HomeAttrController : Controller
	{
		[GET("homeattr/index")]
		public ActionResult Index()
		{
			return new EmptyResult();
		}
	}
}
