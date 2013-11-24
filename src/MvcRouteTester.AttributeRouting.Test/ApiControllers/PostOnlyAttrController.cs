using System.Collections.Generic;
using System.Web.Http;

namespace MvcRouteTester.AttributeRouting.Test.ApiControllers
{
	public class PostOnlyAttrController : ApiController
	{
		[HttpPost]
		[Route("api/postonlyattr/{id}")]
		public IList<int> Post(int id)
		{
			return new List<int> { 1, 2, 3, 4 };
		}
	}
}