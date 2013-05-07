using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class PostDataModel
	{
		public string Name { get; set; }
		public int Number { get; set; }
	}

	public class FromBodyController : Controller
	{
		public ActionResult Post(int id, PostDataModel data)
		{
			return new EmptyResult();
		}
	}
}
