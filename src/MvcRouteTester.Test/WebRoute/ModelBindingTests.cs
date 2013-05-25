using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class ModelBindingTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				name: "TwoParams",
				url: "{controller}/{action}/{id}/{name}",
				defaults: new { controller = "Home", action = "Index" });

			routes.MapRoute(
				name: "OneParam",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}

		[Test]
		public void HasRoute()
		{
			RouteAssert.HasRoute(routes, "/withobject/index/1/fred");
		}

		[Test]
		public void HasRouteToController()
		{
			var expectedRoute = new
				{
					controller = "withobject",
					action = "Index"
				};

			RouteAssert.HasRoute(routes, "/withobject/index/1/fred", expectedRoute);
		}

		[Test]
		public void HasRouteToControllerWithParams()
		{
			var expectedRoute = new
				{
					controller = "withobject",
					action = "Index",
					id = 1,
					name = "fred"
				};

			RouteAssert.HasRoute(routes, "/withobject/index/1/fred", expectedRoute);
		}

		[Test]
		public void FluentBindingTest()
		{
			routes.ShouldMap("/withobject/index/1/fred")
				.To<WithObjectController>(x => x.Index(new InputModel { Id = 1, Name = "fred" }));
		}

		[Test]
		public void HasRouteToNullable()
		{
			RouteAssert.HasRoute(routes, "/withobject/action2/1");
		}

		[Test]
		public void HasRouteToNullableControllerWithParams()
		{
			var expectedRoute = new
				{
					controller = "withobject",
					action = "Action2",
					id = 1
				};

			RouteAssert.HasRoute(routes, "/withobject/Action2/1", expectedRoute);
		}

		[Test]
		public void FluentBindingTestToNullableParamsWithNonNullValue()
		{
			routes.ShouldMap("/withobject/Action2/1")
				.To<WithObjectController>(x => x.Action2(new InputModelWithNullable { Id = 1 }));
		}

		[Test]
		public void FluentBindingTestToNullableParamsWithNullValue()
		{
			routes.ShouldMap("/withobject/Action2")
				.To<WithObjectController>(x => x.Action2(new InputModelWithNullable { Id = null }));
		}

	}
}
