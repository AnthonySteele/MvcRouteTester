using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class SimpleModelBindingTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			config = new HttpConfiguration();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{data}");
		}

		[Test]
		public void TestHasApiRouteToCorrectController()
		{
			var expectations = new
			{
				controller = "WithSimpleObject",
				action = "Get"
			};
			RouteAssert.HasApiRoute(config, "/api/withsimpleobject/1-2", HttpMethod.Get, expectations);
		}

		[Test]
		public void TestHasApiRouteParams()
		{
			var expectations = new
				{
					controller = "WithSimpleObject",
					action = "Get",
					data = "1-2"
				};

			RouteAssert.HasApiRoute(config, "/api/withsimpleobject/1-2", HttpMethod.Get, expectations);
		}

		[Test]
		public void TestFluentMap()
		{
			config.ShouldMap("/api/withsimpleobject/1-2").To<WithSimpleObjectController>(HttpMethod.Get,
				c => c.Get(new SimpleInputModel { X = 1, Y = 2 }));
		}

	}
}
