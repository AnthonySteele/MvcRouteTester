using System.Net.Http;
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

			routes = new RouteCollection();
			routes.MapAttributeRoutesInAssembly(typeof(HomeAttrController));
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
	}
}
