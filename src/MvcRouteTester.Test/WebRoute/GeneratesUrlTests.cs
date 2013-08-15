using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
    [TestFixture]
    public class GenereatedUrlTests
    {
        private RouteCollection routes;

        [SetUp]
        public void MakeRouteTable()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());

            routes = new RouteCollection();
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
            RouteAssert.GeneratesUrl(routes, "/", "Index", "Home");
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
