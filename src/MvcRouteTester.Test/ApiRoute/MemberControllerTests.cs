using System.Net.Http;
using System.Web.Http;
using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;
using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class MemberControllerTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				name: "Member_ChangePassword",
				routeTemplate: "Member/{memberId}/ChangePassword",
				defaults: new { controller = "Member", action = "ChangePassword" }
				);

			config.Routes.MapHttpRoute(
				name: "Member_SomeCalculation",
				routeTemplate: "Member/{id}/IntCalculation",
				defaults: new { controller = "Member", action = "IntCalculation" }
				);
		}

		/// <summary>
		/// This particular controller method is giving rise to a UnaryExpression 
		/// Probably from converting the bool returned from MemberController.ChangePassword to object
		/// So this tests ExpressionReader.UnwrapAction
		/// </summary>
		[Test]
		public void TestChangePasswordWithUnaryExpression()
		{
			config.ShouldMap("/member/1234/ChangePassword?newPassword=new")
				.To<MemberController>(HttpMethod.Get, x => x.ChangePassword(1234, "new"));
		}

		[Test]
		public void TestIntCalculationWithUnaryExpression()
		{
			config.ShouldMap("/member/1234/IntCalculation")
				.To<MemberController>(HttpMethod.Get, x => x.IntCalculation(1234));
		}
	}
}
