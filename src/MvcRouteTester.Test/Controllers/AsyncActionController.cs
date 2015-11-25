using System;
using System.Threading;
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

        public Task<ActionResult> IndexWithCancellationAsync(int id, CancellationToken cancellationToken)
        {
            Func<ActionResult> responseFunc = () => new EmptyResult();
            return Task<ActionResult>.Factory.StartNew(responseFunc, cancellationToken);
        }

        public Task<JsonResult> JsonAsync(int id)
        {
            Func<JsonResult> responseFunc = () => new JsonResult();
            return Task<JsonResult>.Factory.StartNew(responseFunc);
        }

        public Task<JsonResult> JsonWithCancellationAsync(int id, CancellationToken cancellationToken)
        {
            Func<JsonResult> responseFunc = () => new JsonResult();
            return Task<JsonResult>.Factory.StartNew(responseFunc, cancellationToken);
        }
    }
}
