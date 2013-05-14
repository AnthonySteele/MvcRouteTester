using System.Web.Mvc;
using System.Web.Routing;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	public class HasAliasRouteTests
	{
		[Test]
		public void FluentRouteWithCaps()
		{
			var routesWithCaps = new RouteCollection();
			routesWithCaps.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// note that "Controller" and "Action" are capitalised
			routesWithCaps.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { Controller = "Home", Action = "Index", id = 32 });

			RouteAssert.HasRoute(routesWithCaps, "/home/index/32", "Home", "Index");
		}

		[Test]
		public void FluentRouteShouldHandleAliasRoute()
		{
			var routesWithAlias = new RouteCollection();
			routesWithAlias.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routesWithAlias.MapRoute(
				name: "Alias Route",
				url: "aliasroute",
				defaults: new { controller = "Home", action = "Index", id = 32 });

			RouteAssert.HasRoute(routesWithAlias, "/aliasroute", "Home", "Index");
		}

		[Test]
		public void FluentRouteShouldHandleAliasRouteWithCaps()
		{
			var routesWithAlias = new RouteCollection();
			routesWithAlias.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// note that "Controller" and "Action" are capitalised
			routesWithAlias.MapRoute(
				name: "Alias Route",
				url: "aliasroute",
				defaults: new { Controller = "Home", Action = "Index", id = 32 });

			RouteAssert.HasRoute(routesWithAlias, "/aliasroute", "Home", "Index");
		}

	}
}
