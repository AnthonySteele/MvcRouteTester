using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
    public class GetPostController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            return new EmptyResult();
        }
    }
}
