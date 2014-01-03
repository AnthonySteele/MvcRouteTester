using System.Web.Mvc;
using System.Web.Routing;
using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;
using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class RouteConstraintTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				name: "constrained",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index" },
				constraints: new { id = @"\d+" });
		}

		[Test]
		public void HasRouteWhenConstraintIsMatched()
		{
			RouteAssert.HasRoute(routes, "/home/index/1");
			RouteAssert.HasRoute(routes, "/home/index/123456");
		}

		[Test]
		public void HasFluentRouteWhenConstraintIsMatched()
		{
			routes.ShouldMap("/home/index/1").To<HomeController>(x => x.Index(1));
			routes.ShouldMap("/home/index/123456").To<HomeController>(x => x.Index(123456));
		}

		[Test]
		public void DoesNotHaveRouteWhenConstraintisNotMatched()
		{
			RouteAssert.NoRoute(routes, "/home/index/foo");
			RouteAssert.NoRoute(routes, "/home/index/123foo");
		}

		[Test]
		public void DoesNotHaveFluentRouteWhenConstraintisNotMatched()
		{
			routes.ShouldMap("/home/index/foo").ToNoRoute();
			routes.ShouldMap("/home/index/123foo").ToNoRoute();
		}
	}
}
