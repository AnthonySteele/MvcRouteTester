using System.Collections.Generic;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	/// <summary>
	/// This controller is not used to serve data
	/// But the API controller tests do need an actual controller class to be present
	/// as they inspect its public methods to see which Http methods it can respond to
	/// </summary>
	public class PostOnlyController : ApiController
	{
		public IList<int> Post(int id)
		{
			return new List<int> { 1, 2, 3, 4 };
		}
	}
}
