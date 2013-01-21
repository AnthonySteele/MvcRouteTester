using System.Web.Mvc;
using System.Web.Routing;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class HasRouteWithTildeTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}

		[Test]
		public void HasEmptyRoute()
		{
			RouteAssert.HasRoute(routes, "~/");
		}

		[Test]
		public void HasHomeRoute()
		{
			RouteAssert.HasRoute(routes, "~/home");
		}

		[Test]
		public void HasHomeIndexRoute()
		{
			RouteAssert.HasRoute(routes, "~/home/index");
		}

		[Test]
		public void HasHomeIndexWithIdRoute()
		{
			RouteAssert.HasRoute(routes, "~/home/index/1");
		}

		[Test]
		public void DoesNotHaveOtherRoute()
		{
			RouteAssert.NoRoute(routes, "~/foo/bar/fish/spon");
		}
	}
}
