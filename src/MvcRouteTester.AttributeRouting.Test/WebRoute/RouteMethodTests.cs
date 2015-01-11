using System.Web.Mvc.Routing;
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
            var defaultConstraintResolver = new DefaultInlineConstraintResolver();
            defaultConstraintResolver.ConstraintMap.Add("verb", typeof(CustomConstraint));

			routes = new RouteCollection();
			routes.MapAttributeRoutesInAssembly(typeof(HomeAttrController), defaultConstraintResolver);
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
