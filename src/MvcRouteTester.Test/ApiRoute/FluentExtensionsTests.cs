using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class FluentExtensionsTests
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
		public void SimpleTest()
		{
			config.ShouldMap("/api/customer/32").To<CustomerController>(HttpMethod.Get, x => x.Get(32));
		}

		[Test]
		public void TestNoRouteForMethod()
		{
			config.ShouldMap("/api/customer/32").ToNoMethod<CustomerController>(HttpMethod.Post);
		}

		[Test]
		public void TestNoRoute()
		{
			config.ShouldMap("/pai/customer/32").ToNoRoute();
		}
	}
}
