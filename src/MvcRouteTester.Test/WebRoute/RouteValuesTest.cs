using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class RouteValuesTest
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

			routes.MapRoute(
				name: "FooBarFishRoute",
				url: "Foo/Bar/Fish/{name}/{pony}",
				defaults: new { controller = "Foo", action = "Bar" });
		}

		[Test]
		public void HomeRouteCapturesId()
		{
			var expectedRoute = new { controller = "Home", action = "Index", id = 42 };
			RouteAssert.HasRoute(routes, "/home/Index/42", expectedRoute);
		}

		[Test]
		public void RouteWithoutControllerCapturesExpectations()
		{
			// with web routes, you  don't need to find a controller to match a route
			var expectedRoute = new { controller = "foo", action = "bar", id = 42 };
			RouteAssert.HasRoute(routes, "/foo/bar/42", expectedRoute);
		}

		[Test]
		public void RouteCapturesTwoValues()
		{
			var expectedRoute = new { controller = "Foo", action = "Bar", name = "betsy", pony = "trotter" };
			RouteAssert.HasRoute(routes, "/Foo/Bar/Fish/betsy/trotter", expectedRoute);
		}
	}
}
