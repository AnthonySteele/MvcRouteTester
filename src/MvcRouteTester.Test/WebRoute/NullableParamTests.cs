using System.Web.Mvc;
using System.Web.Routing;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class NullableParamTests
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
		public void WorksWithoutIdTested()
		{
			var expectedRoute = new { controller = "WithNullable", action = "Index" };
			RouteAssert.HasRoute(routes, "/WithNullable/index", expectedRoute);
		}

		[Test]
		public void NullValueIsCaptured()
		{
			var expectedRoute = new { controller = "WithNullable", action = "Index", id = (int?)null };
			RouteAssert.HasRoute(routes, "/WithNullable/index", expectedRoute);
		}

		[Test]
		public void NonNullValueIsCaptured()
		{
			var expectedRoute = new { controller = "WithNullable", action = "Index", id = 47 };
			RouteAssert.HasRoute(routes, "/WithNullable/index?id=47", expectedRoute);
		}
	
	}
}
