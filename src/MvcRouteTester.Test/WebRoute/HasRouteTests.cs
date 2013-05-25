using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class HasRouteTests
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
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void HasEmptyRoute()
		{
			RouteAssert.HasRoute(routes, "/");
		}

		[Test]
		public void HasHomeRoute()
		{
			RouteAssert.HasRoute(routes, "/home");
		}

		[Test]
		public void HasHomeIndexRoute()
		{
			RouteAssert.HasRoute(routes, "/home/index");
		}

		[Test]
		public void HasHomeIndexWithIdRoute()
		{
			RouteAssert.HasRoute(routes, "/home/index/1");
		}

		[Test]
		public void HasRouteWithoutController()
		{
			// with web routes, you  don't need to find a controller to match a route
			RouteAssert.HasRoute(routes, "/foo/bar/1");
		}
		
		[Test]
		public void DoesNotHaveOtherRoute()
		{
			RouteAssert.NoRoute(routes, "/foo/bar/fish/spon");
		}

		[Test]
		public void NoFailsOccurOnValidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			RouteAssert.HasRoute(routes, "/home/index/1");

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
			Assert.That(assertEngine.Messages.Count, Is.EqualTo(0));
		}

		[Test]
		public void HasRouteFailsOnInvalidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			RouteAssert.HasRoute(routes, "/foo/bar/fish/spon");

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Should have found the route to '/foo/bar/fish/spon'"));
		}

		[Test] public void NoRouteFailsOnValidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			RouteAssert.NoRoute(routes, "/home/index/1");

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Should not have found the route to '/home/index/1'"));
		}

		[Test]
		public void NoFailsOccurOnNoRouteToInvalidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			RouteAssert.NoRoute(routes, "/foo/bar/fish/spon");

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
			Assert.That(assertEngine.Messages.Count, Is.EqualTo(0));
		}
	}
}
