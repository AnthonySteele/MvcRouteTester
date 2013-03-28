using System.Web.Mvc;
using AttributeRouting.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
    public class HomeController : Controller
    {
        [GET("home/index")]
        public ActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
