using System.Web.Mvc.Routing;
using System.Web.Routing;

using MvcRouteTester.AttributeRouting.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
	[TestFixture]
	public class WithAreaTests
	{
		private RouteCollection routes;
		[SetUp]
		public void Setup()
		{
			var defaultConstraintResolver = new DefaultInlineConstraintResolver();
			defaultConstraintResolver.ConstraintMap.Add("verb", typeof(CustomConstraint));

			routes = new RouteCollection();
			routes.MapAttributeRoutesInAssembly(typeof(WithAreaController).Assembly, defaultConstraintResolver);
		}

		[Test]
		public void TestRouteWithArea()
		{
			routes.ShouldMap("/FooArea/Index").To<WithAreaController>(c => c.Index());
		}
	}
}
