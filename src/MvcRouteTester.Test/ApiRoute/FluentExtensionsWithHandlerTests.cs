using System.Net.Http;
using System.Web.Http;
using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class FluentWithHandlertExtensionsTests
	{
		private class TestHandlerOne : DelegatingHandler
		{
		}

		private class TestHandlerTwo : DelegatingHandler
		{
		}

		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

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
		public void WithHandlerShouldFailWithWrongHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
			config.Routes.Clear();
			AddRouteToTestHandlerOne(config.Routes);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithHandler<TestHandlerTwo>();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Did not match handler type 'TestHandlerTwo' for url 'http://site.com/api/customer/32', found a handler of type 'TestHandlerOne'."));
		}

		[Test]
		public void WithHandlerShouldFailWithNoHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithHandler<TestHandlerTwo>();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Did not match handler type 'TestHandlerTwo' for url 'http://site.com/api/customer/32', found no handler."));
		}

        [Test]
        public void WithHandlerShouldSucceedWithCorrectHandler()
        {
            var assertEngine = new FakeAssertEngine();
            RouteAssert.UseAssertEngine(assertEngine);
            config.Routes.Clear();
            AddRouteToTestHandlerOne(config.Routes);

            config.ShouldMap("/api/customer/32").WithHandler<TestHandlerOne>();

            Assert.That(assertEngine.FailCount, Is.EqualTo(0));
        }

		[Test]
		public void WithHandlerShouldSucceedWithCorrectControllerAndHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
			config.Routes.Clear();
			AddRouteToTestHandlerOne(config.Routes);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithHandler<TestHandlerOne>();

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
		}

		[Test]
		public void WithoutHandlerWithtypeShouldSucceedIfNoHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/customer/32").WithoutHandler<TestHandlerOne>();

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
		}

        [Test]
        public void WithoutHandlerWithtypeShouldSucceedWithcontrollerIfNoHandler()
        {
            var assertEngine = new FakeAssertEngine();
            RouteAssert.UseAssertEngine(assertEngine);

            config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithoutHandler<TestHandlerOne>();

            Assert.That(assertEngine.FailCount, Is.EqualTo(0));
        }

		[Test]
		public void WithoutHandlerShouldSucceedIfNoHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithoutHandler();

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
		}

		[Test]
		public void WithoutHandlerWithTypeShouldSucceedIfDifferentHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
			config.Routes.Clear();
			AddRouteToTestHandlerOne(config.Routes);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithoutHandler<TestHandlerTwo>();

			Assert.That(assertEngine.FailCount, Is.EqualTo(0));
		}

		[Test]
		public void WithoutHandlerShouldFailIfDifferentHandler()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
			config.Routes.Clear();
			AddRouteToTestHandlerOne(config.Routes);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithoutHandler();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Matching handler of type 'TestHandlerOne' found for url 'http://site.com/api/customer/32'."));
		}

		[Test]
		public void WithoutHandlerShouldFailIfMatchingHandlerIsFound()
		{
			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
			config.Routes.Clear();
			AddRouteToTestHandlerOne(config.Routes);

			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32)).WithoutHandler<TestHandlerOne>();

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Matching handler of type 'TestHandlerOne' found for url 'http://site.com/api/customer/32'."));
		}

		private void AddRouteToTestHandlerOne(HttpRouteCollection routes)
		{
			routes.MapHttpRoute(
				name: "ToTestHandlerOne",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional },
				constraints: null,
				handler: new TestHandlerOne());
		}
	}
}
