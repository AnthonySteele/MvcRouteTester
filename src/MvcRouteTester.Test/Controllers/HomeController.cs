using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new EmptyResult();
        }

		public ActionResult Index(int id)
		{
			return new EmptyResult();
		}

	    public ActionResult About()
	    {
	        return new EmptyResult();
	    }
	}
}
