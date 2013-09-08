using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRouteTester.Test.Areas.SomeArea;
using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;
using NUnit.Framework;
using TestController = MvcRouteTester.Test.Controllers.TestController;

namespace MvcRouteTester.Test.WebRoute
{
	public class FluentGenerateUrlTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();

			var areaRegistration = new SomeAreaAreaRegistration();
			var context = new AreaRegistrationContext(areaRegistration.AreaName, routes);
			areaRegistration.RegisterArea(context);

			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute(
				name: "ActionOnly",
				url: "{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = 32 });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void SimpleFluentRoute()
		{
			routes.ShouldMap("/").From<HomeController>(x => x.Index());
		}

		[Test]
		public void Home_About_FluentRoute()
		{
			routes.ShouldMap("/about").From<HomeController>(x => x.About());
		}

		[Test]
		public void TestController_NoParamAction_FluentRoute()
		{
			routes.ShouldMap("/test/noparamaction").From<TestController>(x => x.NoParamAction());
		}

		[Test]
		public void HomeController_Index_Params_FluentRoute()
		{
			routes.ShouldMap("/index/32").From<HomeController>(x => x.Index(32));
		}

		[Test]
		public void TestController_Index_Params_FluentRoute()
		{
			routes.ShouldMap("/test?foo=footest&bar=bartest").From<TestController>(x => x.Index("footest", "bartest"));
		}
		[Test]
		public void TestController_IdAndStringAction_FluentRoute()
		{
			routes.ShouldMap("/test/idandstring/10?foo=footest").From<TestController>(x => x.IdAndString(10, "footest"));
		}

		[Test]
		public void Area_SimpleFluentRoute()
		{
			routes.ShouldMap("/SomeArea").From<Areas.SomeArea.Controllers.TestController>(x => x.Index());
		}

		[Test]
		public void Area_Test_About_FluentRoute()
		{
			routes.ShouldMap("/SomeArea/about").From<Areas.SomeArea.Controllers.TestController>(x => x.About());
		}

		[Test]
		public void Area_TestController_Index_Params_FluentRoute()
		{
			routes.ShouldMap("/SomeArea/index/32").From<Areas.SomeArea.Controllers.TestController>(x => x.Index(32));
		}
		[Test]
		public void Area_OtherTestController_Index()
		{
			routes.ShouldMap("/SomeArea/OtherTest").From<Areas.SomeArea.Controllers.OtherTestController>(x => x.Index());
		}
	}
}
