using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	public class InputModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class WithObjectController : ApiController
	{
		public HttpResponseMessage Get(InputModel data)
		{
			return new HttpResponseMessage();
		}
	}
}
