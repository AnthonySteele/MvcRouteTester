using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class FromBodyTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void TestHasRoute()
		{
			var expectations = new
				{
					controller = "FromBody",
					action = "Post",
					id = "123"
				};

			RouteAssert.HasRoute(routes, "/frombody/post/123", expectations);
		}

		[Test]
		public void TestHasRouteValuesFromBody()
		{
			const string PostBody = "Name=Fred+Bloggers&Number=42";
				var expectations = new
				{
					controller = "FromBody",
					action = "Post",
					id = "123",
					name = "Fred Bloggers",
					number = 42
				};

			RouteAssert.HasRoute(routes, "/frombody/post/123", PostBody, expectations);
		}

		[Test]
		public void MismatchFailsValuesFromBody()
		{
			const string PostBody = "Name=Fred+Bloggers&Number=42";
			var expectations = new
				{
					controller = "FromBody",
					action = "Post",
					id = "123",
					name = "Jim Spriggs",
					number = 42
				};

			var assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			RouteAssert.HasRoute(routes, "/frombody/post/123", PostBody, expectations);

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'Jim Spriggs', not 'Fred Bloggers' for 'name' at url '/frombody/post/123'."));
		}

		[Test]
		public void TestFluentMap()
		{
			PostDataModel postData = null;

			routes.ShouldMap("/frombody/post/123").
				To<FromBodyController>(c => c.Post(123, postData));
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

			routes.ShouldMap("/frombody/post/123").WithBody(PostBody).
				To<FromBodyController>(c => c.Post(123, postData));
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

			routes.ShouldMap("/frombody/post/123").WithBody(PostBody).
				To<FromBodyController>(c => c.Post(123, postData));

			Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'Jim Spriggs', not 'Fred Bloggers' for 'Name' at url '/frombody/post/123'."));
		}
	}
}
