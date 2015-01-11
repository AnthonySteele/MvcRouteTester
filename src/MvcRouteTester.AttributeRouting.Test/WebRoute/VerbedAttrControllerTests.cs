using System.Net.Http;
using System.Web.Mvc.Routing;
using System.Web.Routing;

using MvcRouteTester.AttributeRouting.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
	[TestFixture]
	public class VerbedAttrControllerTests
	{
		private RouteCollection routes;

		[SetUp]
		public void Setup()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

            var defaultConstraintResolver = new DefaultInlineConstraintResolver();
            defaultConstraintResolver.ConstraintMap.Add("verb", typeof(CustomConstraint));

			routes = new RouteCollection();
			routes.MapAttributeRoutesInAssembly(typeof(HomeAttrController), defaultConstraintResolver);
		}

		[Test]
		public void TestFluentRouteGet()
		{
			routes.ShouldMap(HttpMethod.Get, "/verbedattr").To<VerbedAttrController>(x => x.Get());
		}

		[Test]
		public void TestFluentRoutePost()
		{
			routes.ShouldMap(HttpMethod.Post, "/verbedattr").To<VerbedAttrController>(x => x.Post());
		}

		[Test]
		public void TestFluentRouteGetAsync()
		{
			routes.ShouldMap(HttpMethod.Get, "/verbedattrasync").To<VerbedAttrController>(x => x.GetAsync());
		}

		[Test]
		public void TestFluentRoutePostAsync()
		{
			routes.ShouldMap(HttpMethod.Post, "/verbedattrasync").To<VerbedAttrController>(x => x.PostAsync());
		}
	}
}
