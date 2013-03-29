using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using MvcRouteTester.AttributeRouting.Test.Controllers;
using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
    [TestFixture]
    public class HomeControllerTests
    {
        private RouteCollection routes;
        
        [SetUp]
        public void Setup()
        {
            routes = new RouteCollection();
            routes.MapAttributeRoutes(c => c.AddRoutesFromController<HomeController>());
        }

        [Test]
        public void HasRoutesInTable()
        {
            Assert.That(routes.Count, Is.GreaterThan(0));    
        }

        [Test]
        public void HasHomeRoute()
        {
            var expectedRoute = new { controller = "Home", action = "Index" };
            RouteAssert.HasRoute(routes, "/home/index", expectedRoute);    
        }

        [Test]
        public void DoesNotHaveInvalidRoute()
        {
            RouteAssert.NoRoute(routes, "foo/bar/fish");            
        }
    }
}
