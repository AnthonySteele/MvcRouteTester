using System.Web.Mvc;

namespace MvcRouteTester.Test.Areas.SomeArea.Controllers
{
    public class OtherTestController : Controller
    {
        public ActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
