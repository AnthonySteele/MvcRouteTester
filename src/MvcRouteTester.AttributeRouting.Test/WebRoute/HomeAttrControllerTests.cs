using System.Web.Routing;
using System.Web.Mvc;

using MvcRouteTester.AttributeRouting.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
	[TestFixture]
	public class HomeControllerTests
	{
		private RouteCollection routes;
		
		[SetUp]
		public void Setup()
		{
			routes = new RouteCollection();
			routes.MapMvcAttributeRoutes();
		}

		[Test]
		public void HasRoutesInTable()
		{
			Assert.That(routes.Count, Is.GreaterThan(0));
		}

		[Test]
		public void HasHomeRoute()
		{
			var expectedRoute = new { controller = "HomeAttr", action = "Index" };
			RouteAssert.HasRoute(routes, "/homeattr/index", expectedRoute);
		}

		[Test]
		public void DoesNotHaveInvalidRoute()
		{
			RouteAssert.NoRoute(routes, "foo/bar/fish");
		}

		[Test]
		public void HasFluentRoute()
		{
			routes.ShouldMap("/homeattr/index").To<HomeAttrController>(x => x.Index());
		}

		[Test]
		public void HasFluentNoRoute()
		{
			routes.ShouldMap("/foo/bar/fish").ToNoRoute();
		}
	}
}
