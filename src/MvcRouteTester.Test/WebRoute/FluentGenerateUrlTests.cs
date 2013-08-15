using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;
using NUnit.Framework;

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
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
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
			routes.ShouldMap("/home/index/32").To<HomeController>(x => x.Index(32));
		}

		[Test]
		public void SimpleFluentRoute2()
		{
			routes.ShouldMap("/home/index/32").From<HomeController>(x => x.Index(32));
		}

    }
}
