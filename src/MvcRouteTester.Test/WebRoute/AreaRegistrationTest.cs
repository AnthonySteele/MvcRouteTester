using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Areas.SomeArea;
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
			routes.ShouldMap("/SomeArea/Test").To<Areas.SomeArea.TestController>(x => x.Index());
			routes.ShouldMap("/SomeArea/Test/Index").To<Areas.SomeArea.TestController>(x => x.Index());
		}
	}
}
