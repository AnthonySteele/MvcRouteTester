using System.Collections.Generic;
using System.Web.Http;
namespace MvcRouteTester.AttributeRouting.Test.ApiControllers
{
	/// <summary>
	/// This controller is not used to serve data
	/// But the API controller tests do need an actual controller class to be present
	/// as they inspect its public methods to see which Http methods it can respond to
	/// </summary>
	public class CustomerController : ApiController
	{
        [HttpGet]
		[Route("api/customer/{id}")]
		public IList<int> Get(int id) 
		{
			return new List<int> { 1, 2, 3, 4 };
		}
	}
}
