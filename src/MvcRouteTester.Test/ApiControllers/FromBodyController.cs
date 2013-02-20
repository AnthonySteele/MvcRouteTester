using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	public class PostDataModel
	{
		public string Name { get; set; }
		public int Number { get; set; }
	}

	public class FromBodyController : ApiController
	{
		[HttpPost]
		public HttpResponseMessage CreateSomething(int id, [FromBody] PostDataModel data)
		{
			return new HttpResponseMessage();
		}
	}
}
