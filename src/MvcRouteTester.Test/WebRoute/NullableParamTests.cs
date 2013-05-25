using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;

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
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

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

		[Test]
		public void FluentMapToNull()
		{
			routes.ShouldMap("/WithNullable/index").To<WithNullableController>(x => x.Index(null));
		}
	
		[Test]
		public void FluentMapToNotNull()
		{
			routes.ShouldMap("/WithNullable/index?id=47").To<WithNullableController>(x => x.Index(47));
		}
	}
}
