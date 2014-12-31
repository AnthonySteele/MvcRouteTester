using System.Web.Mvc.Routing;
using System.Web.Routing;

using MvcRouteTester.AttributeRouting.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
    [TestFixture]
    public class InlineConstraintControllerTests
    {
        private RouteCollection routes;

        [SetUp]
        public void Setup()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());

            var defaultConstraintResolver = new DefaultInlineConstraintResolver();
            defaultConstraintResolver.ConstraintMap.Add("verb", typeof(CustomConstraint));

            routes = new RouteCollection();
            routes.MapAttributeRoutesInAssembly(typeof(InlineConstraintController), defaultConstraintResolver);
        }

        [Test]
        public void HasRoutesInTable()
        {
            Assert.That(routes.Count, Is.GreaterThan(0));
        }

        [Test]
        public void HasHomeRoute()
        {
            var expectedRoute = new { controller = "InlineConstraint", action = "Index" };
            RouteAssert.HasRoute(routes, "/inlineconstraint/drawing", expectedRoute);
        }

        [Test]
        public void DoesNotHaveInvalidRoute()
        {
            RouteAssert.NoRoute(routes, "/inlineconstraint/draw");
        }

        [Test]
        public void HasFluentRoute()
        {
            routes.ShouldMap("/inlineconstraint/drawing").To<InlineConstraintController>(x => x.Index("drawing"));
        }

        [Test]
        public void HasFluentNoRoute()
        {
            routes.ShouldMap("/inlineconstraint/draw").ToNoRoute();
        }
    }
}
