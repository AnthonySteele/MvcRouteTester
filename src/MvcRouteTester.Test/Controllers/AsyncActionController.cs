using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MvcRouteTester.Test.Controllers
{
	public class AsyncActionController : Controller
	{
		public Task<ActionResult> IndexAsync(int id)
		{
			Func<ActionResult> responseFunc = () => new EmptyResult();
			return Task<ActionResult>.Factory.StartNew(responseFunc);
		}
	}
}
