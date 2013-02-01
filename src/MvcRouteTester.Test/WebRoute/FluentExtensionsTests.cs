using System.Web.Mvc;
using System.Web.Routing;

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
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = 32 });
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

	}
}
