using System.Web.Routing;
using AttributeRouting.Web.Mvc;
using MvcRouteTester.AttributeRouting.Test.Controllers;
using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
	[TestFixture]
	public class RouteMethodTests
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
	}
}
