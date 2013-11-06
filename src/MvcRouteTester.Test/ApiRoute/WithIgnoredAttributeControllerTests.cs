using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class WithIgnoredAttributeControllerTests
	{
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

		[Test]
		public void ShouldHaveRoute()
		{
			var expectedRoute = new { controller = "withignoredattribute", action = "Get" };

			RouteAssert.HasApiRoute(config, "/api/withignoredattribute/1", HttpMethod.Get, expectedRoute);
		}

		[Test]
		public void ShouldFailOnIgnoredAttributeWhichIsNotRegistered()
		{
			var expectedModel = new TestModel
				{
					Id = 42,
					Ignored = "Failed value"
				};

			var testEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(testEngine);
			RouteAssert.ClearIgnoreAttributes();

			config.ShouldMap("/api/withignoredattribute/42").To<WithIgnoredAttributeController>(
				HttpMethod.Get, x => x.Get(expectedModel));

			Assert.That(testEngine.FailCount, Is.EqualTo(1));
			Assert.That(testEngine.Messages[0], Is.EqualTo("Expected 'Failed value', got missing value for 'Ignored' at url '/api/withignoredattribute/42'."));
		}

		[Test]
		public void ShouldNotMapIgnoredAttributeWhenRegistered()
		{
			var expectedModel = new TestModel
				{
					Id = 42,
					Ignored = "Anything at all"
				};

			RouteAssert.AddIgnoreAttribute(typeof(IgnoreMeAttribute));

			config.ShouldMap("/api/withignoredattribute/42").To<WithIgnoredAttributeController>(
				HttpMethod.Get, x => x.Get(expectedModel));
		}
	}
}
