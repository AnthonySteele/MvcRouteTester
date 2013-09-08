using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Areas.SomeArea;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class HasRouteToAreaTests
	{
		private RouteCollection routes;
		
		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();

			var areaRegistration = new SomeAreaAreaRegistration();
			var context = new AreaRegistrationContext(areaRegistration.AreaName, routes);
			areaRegistration.RegisterArea(context);
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void HasDefaultRouteToArea()
		{
			RouteAssert.HasRoute(routes, "/SomeArea");
		}

		[Test]
		public void AreaRouteHasControllerAndAction()
		{
			RouteAssert.HasRoute(routes, "/SomeArea", new { Controller = "Test", Action = "Index" });
		}

		[Test]
		public void AreaRouteHasAreaName()
		{
			RouteAssert.HasRoute(routes, "/SomeArea", new { Area = "SomeArea", Controller = "Test", Action = "Index" });
		}
	}
}
