using System.Net.Http;
using System.Web.Http;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class HasRouteTests
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
		public void HasApiRoute()
		{
			RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get);
		}

		[Test]
		public void HasApiRouteWithExpectation()
		{
			var expectation = new { controller = "Customer", action= "get", id = "1" };
			RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get, expectation);
		}

		[Test]
		public void CustomerControllerDoesNotHavePostMethod()
		{
			RouteAssert.ApiRouteDoesNotHaveMethod(config, "~/api/customer/1", HttpMethod.Post);
		}

		[Test]
		public void ShouldNotFindNonexistentRoute()
		{
			RouteAssert.NoApiRoute(config, "~/pai/customer/1");
		}
	}
}
