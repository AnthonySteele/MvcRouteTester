using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;
using MvcRouteTester.AttributeRouting.Test.ApiControllers;
using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.ApiRoute
{
	[TestFixture]
	public class CustomerControllerTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			config = new HttpConfiguration();
			config.Routes.MapHttpAttributeRoutes(c =>
				{
					c.InMemory = true;
					c.AddRoutesFromController<CustomerController>();
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
			var expectations = new { controller = "Customer", action = "get", id = "1" };
			RouteAssert.HasApiRoute(config, "/api/customer/1", HttpMethod.Get, expectations);
		}

		[Test]
		public void DoesNotHaveInvalidRoute()
		{
			RouteAssert.NoApiRoute(config, "/foo/bar/fish");
		}

	}
}
