using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	public class AsyncActionController : ApiController
	{
		public Task<HttpResponseMessage> GetAsync(int id)
		{
			Func<HttpResponseMessage> responseFunc = () => new HttpResponseMessage();
			return Task<HttpResponseMessage>.Factory.StartNew(responseFunc);
		}
	}
}
