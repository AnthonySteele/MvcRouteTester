using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;
using MvcRouteTester.AttributeRouting.Test.ApiControllers;
using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.ApiRoute
{
	[TestFixture]
	public class CustomerAttrControllerTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			config = new HttpConfiguration();
			config.Routes.MapHttpAttributeRoutes(c =>
				{
					c.InMemory = true;
					c.AddRoutesFromController<CustomerAttrController>();
				});
		}

		[Test]
		public void HasRoutesInTable()
		{
			Assert.That(config.Routes.Count, Is.GreaterThan(0));
		}

		[Test]
		public void HasApiRoute()
		{
			RouteAssert.HasApiRoute(config, "/api/customerattr/1", HttpMethod.Get);
		}
		
		[Test]
		public void HasApiRouteWithExpectations()
		{
			var expectations = new { controller = "CustomerAttr", action = "get", id = "1" };
			RouteAssert.HasApiRoute(config, "/api/customerattr/1", HttpMethod.Get, expectations);
		}

		[Test]
		public void DoesNotHaveInvalidRoute()
		{
			RouteAssert.NoApiRoute(config, "/foo/bar/fish");
		}

		[Test]
		public void HasFluentRoute()
		{
			config.ShouldMap("/api/customerattr/1").To<CustomerAttrController>(HttpMethod.Get, x => x.Get(1));
		}

	}
}
