using System.Web.Mvc;
using System.Web.Routing;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class DefaultsTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = 32 });
		}

		[Test]
		public void HasRouteWithAllValuesSpecified()
		{
			var expectedRoute = new { controller = "Home", action = "Index", id = "42" };
			RouteAssert.HasRoute(routes, "/home/index/42", expectedRoute);
		}

		[Test]
		public void HasRouteWithDefaultId()
		{
			var expectedRoute = new { controller = "Home", action = "Index", id = "32" };
			RouteAssert.HasRoute(routes, "/home/index", expectedRoute);
		}

		[Test]
		public void HasRouteWithDefaultActionAndId()
		{
			var expectedRoute = new { controller = "Home", action = "Index", id = "32" };
			RouteAssert.HasRoute(routes, "/home", expectedRoute);
		}
		[Test]
		public void HasRouteWithAllDefaults()
		{
			var expectedRoute = new { controller = "Home", action = "Index", id = "32" };
			RouteAssert.HasRoute(routes, "/", expectedRoute);
		}
	}
}
