using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class RouteMethodTests
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

		[Test]
		public void HasRouteWithoutId()
		{
			var expectedRoute = new { controller = "GetPost", action = "Index" };
			RouteAssert.HasRoute(routes, "/getpost/index", expectedRoute);
		}

		[Test]
		public void HasRouteWithId()
		{
			var expectedRoute = new { controller = "GetPost", action = "Index", id = "1" };
			RouteAssert.HasRoute(routes, "/getpost/index/1", expectedRoute);
		}
	}
}
