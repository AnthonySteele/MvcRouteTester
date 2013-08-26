using System;
using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	/// <summary>
	/// Tests on model binding and aprams using RouteParamsCasesController
	/// </summary>
	[TestFixture]
	public class RouteParamsCasesTests
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
			var expectedRoute = new { controller = "RouteParamsCases", action = "TwoIntAction", id1 = 42, id2 = 312 };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/TwoIntAction/42/312", expectedRoute);
		}

		[Test]
		public void RouteAssertFailsIfIntAreInWrongPositions()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			var expectedRoute = new { controller = "RouteParamsCases", action = "TwoIntAction", id1 = 42, id2 = 312 };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/TwoIntAction/312/42", expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(2), "Different ints should not match");
		}

		[Test]
		public void RouteCanHaveGuidAsId()
		{
			var guid = Guid.NewGuid();

			var expectedRoute = new { controller = "RouteParamsCases", action = "GuidAction", id = guid };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/GuidAction/" + guid, expectedRoute);
		}

		[Test]
		public void RouteAssertFailsIfGuidDoesNotMatch()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			var guid1 = Guid.NewGuid();
			var guid2 = Guid.NewGuid();

			var expectedRoute = new { controller = "RouteParamsCases", action = "GuidAction", id = guid1 };

			RouteAssert.HasRoute(routes, "/RouteParamsCases/GuidAction/" + guid2, expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1), "Different guids should not match");
		}
	}
}

