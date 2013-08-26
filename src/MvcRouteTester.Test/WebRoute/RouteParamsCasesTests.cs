using System;
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
				name: "WithNameId",
				url: "{controller}/{action}/{id}/{name}",
				defaults: new { controller = "Home", action = "Index" });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
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

		[Test]
		public void RouteCanHaveBoolAsParam()
		{
			var expectedRoute = new { controller = "RouteParamsCases", action = "BoolAction", id = true };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/BoolAction/true", expectedRoute);
		}

		[Test]
		public void RouteAssertFailsIfBoolDoesNotMatch()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);


			var expectedRoute = new { controller = "RouteParamsCases", action = "BoolAction", id = true };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/BoolAction/false", expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1), "Different bools should not match");
		}

		[Test]
		public void RouteCanHaveStructAsParam()
		{
			var expectedRoute = new { controller = "RouteParamsCases", action = "StructAction", id = 42, name = "fred" };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/StructAction/42/fred", expectedRoute);
		}

		[Test]
		public void RouteAssertFailsIfSTructPropertyDoesNotMatch()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);


			var expectedRoute = new { controller = "RouteParamsCases", action = "StructAction", id = 42, name = "fred" };
			RouteAssert.HasRoute(routes, "/RouteParamsCases/StructAction/42/jim", expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1), "Different struct values should not match");
		}
	}
}

