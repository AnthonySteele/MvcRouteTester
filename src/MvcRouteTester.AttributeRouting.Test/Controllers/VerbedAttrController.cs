using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
	public class VerbedAttrController : Controller
	{
		[HttpGet]
		[Route("VerbedAttr")]
		public ActionResult Get()
		{
			return new EmptyResult();
		}

		[HttpGet]
		[Route("VerbedAttrAsync")]
		public async Task<ActionResult> GetAsync()
		{
			return await Task.FromResult(new EmptyResult());
		}

		[HttpPost]
		[Route("VerbedAttr")]
		public ActionResult Post()
		{
			return new EmptyResult();
		}

		[HttpPost]
		[Route("VerbedAttrAsync")]
		public async Task<ActionResult> PostAsync()
		{
			return await Task.FromResult(new EmptyResult());
		}
	}
}
