using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	/// <summary>
	/// Tests on model binding and params using RouteParamsCasesController
	/// </summary>
	[TestFixture]
	public class TwoRouteParamsTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				name: "TwoParams",
				url: "{controller}/{action}/{id1}/{id2}",
				defaults: new { controller = "Home", action = "Index" });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void RouteCanHaveTwoIntIds()
		{
			var expectedRoute = new { controller = "TwoRouteParams", action = "TwoIntAction", id1 = 42, id2 = 312 };
			RouteAssert.HasRoute(routes, "/TwoRouteParams/TwoIntAction/42/312", expectedRoute);
		}

		[Test]
		public void RouteCanHaveTwoStringIds()
		{
			var expectedRoute = new { controller = "TwoRouteParams", action = "TwoStringAction", id1 = "foo", id2 = "bar" };
			RouteAssert.HasRoute(routes, "/TwoRouteParams/TwoStringAction/foo/bar", expectedRoute);
		}

		[Test]
		public void RouteAssertFailsIfIntsAreInWrongPositions()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			var expectedRoute = new { controller = "TwoRouteParams", action = "TwoIntAction", id1 = 42, id2 = 312 };
			RouteAssert.HasRoute(routes, "/TwoRouteParams/TwoIntAction/312/42", expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(2), "Different ints should not match");
		}

		[Test]
		public void RouteAssertFailsIfStringsAreInWrongPositions()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			var expectedRoute = new { controller = "TwoRouteParams", action = "TwoStringAction", id1 = "foo", id2 = "bar" };
			RouteAssert.HasRoute(routes, "/TwoRouteParams/TwoStringAction/bar/foo", expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(2), "Different strings should not match");
		}
	}
}

