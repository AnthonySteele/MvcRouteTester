using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Assertions;
using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class FluentExtensionsTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			routes = new RouteCollection();
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = 32 });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void SimpleFluentRoute()
		{
			routes.ShouldMap("/home/index/32").To<HomeController>(x => x.Index(32));
		}

		[Test]
		public void DefaultFluentRoute()
		{
			routes.ShouldMap("/").To<HomeController>(x => x.Index(32));
		}

		[Test]
		public void SimpleFluentRouteWithParams()
		{
			routes.ShouldMap("/home/index/32?foo=bar").To<HomeController>(x => x.Index(32));
		}

		[Test]
		public void FluentRouteFailsOnWrongRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			routes.ShouldMap("/chome/index/32").To<HomeController>(x => x.Index(32));

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'Home', not 'chome' for 'controller' at url '/chome/index/32'."));
		}

		[Test]
		public void IgnoredRoute()
		{
			routes.ShouldMap("fred.axd").ToIgnoredRoute();
		}

		[Test]
		public void IgnoredRouteFailsOnValidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			routes.ShouldMap("/").ToIgnoredRoute();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Route to '/' is not ignored"));
		}

		[Test]
		public void NonIgnoredRoute()
		{
			routes.ShouldMap("/home/index/32?foo=bar").ToNonIgnoredRoute();
		}

		[Test]
		public void NonIgnoredRouteRouteFailsOnInvalidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			routes.ShouldMap("fred.axd").ToNonIgnoredRoute();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Route to 'fred.axd' is ignored"));
		}

		[Test]
		public void NoRoute()
		{
			routes.ShouldMap("/foo/bar/fish/spon").ToNoRoute();
		}

		[Test]
		public void NoRouteFailsOnValidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			routes.ShouldMap("/home/index").ToNoRoute();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Should not have found the route to '/home/index'"));
		}
	}
}
