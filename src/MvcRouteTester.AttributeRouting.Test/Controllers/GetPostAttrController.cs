using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class GetPostAttrController : Controller
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

        [HttpPost]
        [Route("GetPostAttr/indexasync/{id}")]
        public async Task<ActionResult> IndexAsync(int id)
        {
            return await Task<ActionResult>.FromResult(new EmptyResult());
        }
	}
}
