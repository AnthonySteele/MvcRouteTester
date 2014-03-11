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
			routes = new RouteCollection();
			routes.MapAttributeRoutesInAssembly(typeof(WithAreaController).Assembly);
		}

		[Test]
		public void TestRouteWithArea()
		{
			routes.ShouldMap("/FooArea/Index").To<WithAreaController>(c => c.Index());
		}
	}
}
