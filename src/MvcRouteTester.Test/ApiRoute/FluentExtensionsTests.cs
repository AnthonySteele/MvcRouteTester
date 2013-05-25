using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class FluentExtensionsTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			config = new HttpConfiguration();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void SimpleTest()
		{
			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32));
		}

		[Test]
		public void ShouldMapToFailsWithWrongRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/missing/32/foo").To<CustomerController>(HttpMethod.Get, x => x.Get(32));

			Assert.That(assertEngine.FailCount, Is.EqualTo(2));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("No route matched url 'http://site.com/api/missing/32/foo'"));
		}

		[Test]
		public void TestNoRouteForMethod()
		{
			config.ShouldMap("/api/customer/32").ToNoMethod<CustomerController>(HttpMethod.Post);
		}

		[Test]
		public void ShouldMapToNoMethodFailsOnValidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/customer/32").ToNoMethod<CustomerController>(HttpMethod.Get);

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Method GET is allowed on url '/api/customer/32'"));
		}

		[Test]
		public void TestNoRoute()
		{
			config.ShouldMap("/pai/customer/32").ToNoRoute();
		}

		[Test]
		public void ShouldMapToNoRouteFailsOnValidRoute()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/customer/32").ToNoRoute();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Found a route for url '/api/customer/32'"));
		}
	}
}
