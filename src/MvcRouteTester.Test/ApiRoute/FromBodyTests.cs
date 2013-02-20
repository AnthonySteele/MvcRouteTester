using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;

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
		public void TestFluentMap()
		{
			config.ShouldMap("/api/frombody/123").To<FromBodyController>(HttpMethod.Post,
				c => c.CreateSomething(123, new PostDataModel()));
		}
	}
}
