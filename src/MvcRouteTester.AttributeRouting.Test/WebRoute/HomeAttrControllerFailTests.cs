using System.Web.Mvc.Routing;
using System.Web.Routing;

using MvcRouteTester.AttributeRouting.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
	[TestFixture]
	public class HomeAttrControllerFailTests
	{
		private FakeAssertEngine assertEngine;
		private RouteCollection routes;

		[SetUp]
		public void Setup()
		{
			assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			var defaultConstraintResolver = new DefaultInlineConstraintResolver();
			defaultConstraintResolver.ConstraintMap.Add("verb", typeof(CustomConstraint));

			routes = new RouteCollection();
			routes.MapAttributeRoutesInAssembly(typeof(HomeAttrController), defaultConstraintResolver);
		}

		[Test]
		public void TestNameMismatches()
		{
			var expectedRoute = new { controller = "HoomAttr", action = "Mindex" };
			RouteAssert.HasRoute(routes, "/homeattr/index", expectedRoute);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(2));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'HoomAttr', not 'HomeAttr' for 'controller' at url '/homeattr/index'."));
			Assert.That(assertEngine.Messages[1], Is.EqualTo("Expected 'Mindex', not 'Index' for 'action' at url '/homeattr/index'."));
		}

		[Test]
		public void TestWrongNoRoute()
		{
			RouteAssert.NoRoute(routes, "/homeattr/index");

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Should not have found the route to '/homeattr/index'"));
		}

		[Test]
		public void TestFluentRouteMismatch()
		{
			routes.ShouldMap("/hoomattr/mindex").To<HomeAttrController>(x => x.Index());

			Assert.That(assertEngine.FailCount, Is.GreaterThan(0));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Should have found the route to '/hoomattr/mindex'"));
		}

		[TearDown]
		public void Teardown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}
	}
}
