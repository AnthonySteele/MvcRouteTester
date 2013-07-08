using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	/// <summary>
	/// testing return of value types like int and bool
	/// </summary>
	public class MemberController: ApiController
	{
		[HttpGet]
		public bool ChangePassword(int memberId, string newPassword)
		{
			return memberId != 1;
		}

		[HttpGet]
		public int IntCalculation(int id)
		{
			return id * 42;
		}

		[HttpGet]
		public void DoNothing()
		{
			// testing void return
		}
	}
}
