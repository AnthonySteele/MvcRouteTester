using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Areas.SomeArea;
using MvcRouteTester.Test.Areas.SomeArea.Controllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class AreaRegistrationTest
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();

			var areaRegistration = new SomeAreaAreaRegistration();
			AreaRegistrationContext context = new AreaRegistrationContext(areaRegistration.AreaName, routes);
			areaRegistration.RegisterArea(context);
		}

		[Test]
		public void AreaRegistrationHasRoutes()
		{
			routes.ShouldMap("/SomeArea/").To<Areas.SomeArea.Controllers.TestController>(x => x.Index());
			routes.ShouldMap("/SomeArea/Index").To<Areas.SomeArea.Controllers.TestController>(x => x.Index());
		}

		[Test]
		public void Other_Test_Controller_Routes()
		{
			routes.ShouldMap("/SomeArea/OtherTest").To<OtherTestController>(x => x.Index());
		}
	}
}
