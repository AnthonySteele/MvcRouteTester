using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class FromBodyTests
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
		public void TestHasApiRoute()
		{
			var expectations = new
				{
					controller = "FromBody",
					action = "CreateSomething",
					id = "123"
				};
			RouteAssert.HasApiRoute(config, "/api/frombody/123", HttpMethod.Post, expectations);
		}

		[Test]
		public void TestHasApiRouteValuesFromBody()
		{
			const string PostBody = "Name=Fred+Bloggers&Number=42";
			var expectations = new
			{
				controller = "FromBody",
				action = "CreateSomething",
				id = "123",
				name = "Fred Bloggers",
				number = 42
			};
			RouteAssert.HasApiRoute(config, "/api/frombody/123", HttpMethod.Post, PostBody, expectations);
		}

		[Test]
		public void MismatchFailsValuesFromBody()
		{
			const string PostBody = "Name=Fred+Bloggers&Number=42";
			var expectations = new
			{
				controller = "FromBody",
				action = "CreateSomething",
				id = "123",
				name = "Jim Spriggs",
				number = 42
			};

			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			RouteAssert.HasApiRoute(config, "/api/frombody/123", HttpMethod.Post, PostBody, expectations);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'Jim Spriggs', not 'Fred Bloggers' for 'name' at url '/api/frombody/123'."));
		}

		[Test]
		public void TestFluentMap()
		{
			PostDataModel postData = null;

			config.ShouldMap("/api/frombody/123").
				To<FromBodyController>(HttpMethod.Post, c => c.CreateSomething(123, postData));
		}

		[Test]
		public void TestFluentMapWithBody()
		{
			var postData = new PostDataModel
			{
				Name = "Fred Bloggers",
				Number = 42
			};
			const string PostBody = "Name=Fred+Bloggers&Number=42";

			config.ShouldMap("/api/frombody/123").WithBody(PostBody).
				To<FromBodyController>(HttpMethod.Post, c => c.CreateSomething(123, postData));
		}

		[Test]
		public void MismatchFluentMapWithBodyFails()
		{
			var postData = new PostDataModel
			{
				Name = "Jim Spriggs",
				Number = 42
			};
			const string PostBody = "Name=Fred+Bloggers&Number=42";

			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			config.ShouldMap("/api/frombody/123").WithBody(PostBody).
				To<FromBodyController>(HttpMethod.Post, c => c.CreateSomething(123, postData));

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'Jim Spriggs', not 'Fred Bloggers' for 'name' at url '/api/frombody/123'."));
		}
	}
}
