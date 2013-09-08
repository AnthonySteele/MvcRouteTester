using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRouteTester.Test.Areas.SomeArea;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class GeneratesUrlTests
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

			routes.MapRoute(
				name: "ActionOnly",
				url: "{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void Can_Generate_Root_Url_From_Home_Path()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", "Index", "Home");
		}

		[Test]
		public void Can_Generate_Root_Url_From_Home_Path_with_anon_object()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", new { action = "Index", controller = "Home" });
		}

		[Test]
		public void Can_Generate_Area_Root_Url_From_Area_Home_Path_with_anon_object()
		{
			RouteAssert.GeneratesActionUrl(routes, "/SomeArea", new { action = "Index", controller = "Test", area = "SomeArea" });
		}
		[Test]
		public void Can_Generate_Area_Url_From_SomeAreaAction_Path_with_anon_object()
		{
            RouteAssert.GeneratesActionUrl(routes, "/SomeArea/About", new { action = "About", controller = "Test", area = "SomeArea" });
		}

		[Test]
		public void Can_Generate_Root_Url_From_Home_Path_with_dictionary()
		{
			var values = new Dictionary<string, string>
				{
					{ "action", "Index"},
					{ "controller", "Home"},
				};

			RouteAssert.GeneratesActionUrl(routes, "/", values);
		}

		[Test]
		public void Can_Generate_Root_Url_with_app_path()
		{
			var values = new Dictionary<string, string>
				{
					{ "action", "Index"},
					{ "controller", "Home"},
				};

			RouteAssert.GeneratesActionUrl(routes, "/appPath/", values, "/appPath");
		}

		[Test]
		public void Can_Generate_Root_Url_with_http_post()
		{
			var values = new Dictionary<string, string>
				{
					{ "action", "Index"},
					{ "controller", "Home"},
				};

			RouteAssert.GeneratesActionUrl(routes, HttpMethod.Post, "/", values);
		}

		[Test]
		public void Can_Generate_Action_Url_From_Action()
		{
			RouteAssert.GeneratesActionUrl(routes, "/TestAction", "TestAction");
		}

		[Test]
		public void Can_Generate_Action_Url_From_HomeController_IndexAction()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", "Index", "Home");
		}

		[Test]
		public void can_generate_action_url_from_action_case_insensitive()
		{
			RouteAssert.GeneratesActionUrl(routes, "/tEst", "Test");
		}

		[Test]
		public void Can_Generate_Action_Url_From_CustomController()
		{
			RouteAssert.GeneratesActionUrl(routes, "/CustomController", "Index", "CustomController");
		}

		[Test]
		public void Can_Generate_Action_Url_From_CustomController_CustomAction()
		{
			RouteAssert.GeneratesActionUrl(routes, "/CustomController/Test", "Test", "CustomController");
		}
	}
}
