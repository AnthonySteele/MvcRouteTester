using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class IgnoredRouteTest
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
		}

		[Test]
		public void IgnoredRouteIsFound()
		{
			RouteAssert.HasRoute(routes, "fred.axd");
		}

		[Test]
		public void NormalRouteIsFound()
		{
			RouteAssert.HasRoute(routes, "foo/bar/1");
		}

		[Test]
		public void IgnoredRouteIsIgnored()
		{
			RouteAssert.IsIgnoredRoute(routes, "fred.axd");
		}

		[Test]
		public void NormalRouteIsNotIgnored()
		{
			RouteAssert.IsNotIgnoredRoute(routes, "foo/bar/1");
		}
	}
}
