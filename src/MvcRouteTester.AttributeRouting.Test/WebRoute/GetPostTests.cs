using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using MvcRouteTester.AttributeRouting.Test.Controllers;
using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
    [TestFixture]
    public class GetPostTests
    {
        private RouteCollection routes;

        [SetUp]
        public void Setup()
        {
            routes = new RouteCollection();
            routes.MapAttributeRoutes(c => c.AddRoutesFromController<GetPostController>());
        }

        [Test]
        public void HasRouteWithoutId()
        {
            var expectedRoute = new { controller = "GetPost", action = "Index" };
            RouteAssert.HasRoute(routes, "/getpost/index", expectedRoute);
        }

        [Test]
        public void HasGetRouteWithoutId()
        {
            RouteAssert.RouteHasMethod(routes, "/getpost/index", HttpMethod.Get);
        }

        [Test]
        public void NoPostRouteWithoutId()
        {
            RouteAssert.RouteDoesNotHaveMethod(routes, "/getpost/index", HttpMethod.Post);
        }

        [Test]
        public void HasRouteWithId()
        {
            var expectedRoute = new { controller = "GetPost", action = "Index", id = "1" };
            RouteAssert.HasRoute(routes, "/getpost/index/1", expectedRoute);
        }

        [Test]
        public void HasPostRouteWithId()
        {
            RouteAssert.RouteHasMethod(routes, "/getpost/index/1", HttpMethod.Post);
        }

        [Test]
        public void NoGetRouteWithId()
        {
            RouteAssert.RouteDoesNotHaveMethod(routes, "/getpost/index/1", HttpMethod.Get);
        }
    }
}
