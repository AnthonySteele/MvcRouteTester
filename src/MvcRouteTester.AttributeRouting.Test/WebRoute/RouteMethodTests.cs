using System.Web.Routing;

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
			routes.MapAttributeRoutesInAssembly(typeof(HomeAttrController));
		}

		[Test]
		public void HasRouteWithoutId()
		{
			var expectedRoute = new { controller = "GetPostAttr", action = "Index" };
			RouteAssert.HasRoute(routes, "/getpostattr/index", expectedRoute);
		}

		[Test]
		public void HasFluentRoute()
		{
			routes.ShouldMap("/getpostattr/index").To<GetPostAttrController>(x => x.Index());
		}
	}
}
