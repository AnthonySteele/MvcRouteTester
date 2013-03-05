using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class ModelBindingTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			config = new HttpConfiguration();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}/{name}",
				defaults: new { id = RouteParameter.Optional });
		}

		[Test]
		public void TestHasApiRouteToCorrectController()
		{
			var expectations = new
			{
				controller = "WithObject",
				action = "Get"
			};
			RouteAssert.HasApiRoute(config, "/api/withobject/123/fred", HttpMethod.Get, expectations);
		}

		[Test]
		public void TestHasApiRouteParams()
		{
			var expectations = new
				{
					controller = "WithObject",
					action = "Get",
					id = "123",
					name = "fred"
				};

			RouteAssert.HasApiRoute(config, "/api/withobject/123/fred", HttpMethod.Get, expectations);
		}

		[Test, Ignore("not working yet")]
		public void TestFluentMap()
		{
			config.ShouldMap("/api/withobject/123/fred").To<WithObjectController>(HttpMethod.Get,
				c => c.Get(new InputModel()));
		}

	}
}
