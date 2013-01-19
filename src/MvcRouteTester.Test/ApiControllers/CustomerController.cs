using System.Collections.Generic;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	/// <summary>
	/// This class is not used to serve data
	/// But the API controller tests do nned an actuall controller class to be present
	/// </summary>
	public class CustomerController : ApiController
	{
		public IList<int> Get()
		{
			return new List<int> { 1, 2, 3, 4 };
		}
	}
}
