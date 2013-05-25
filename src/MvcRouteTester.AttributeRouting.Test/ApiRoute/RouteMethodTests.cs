using System.Net.Http;
using System.Web.Http;
using AttributeRouting.Web.Http.WebHost;
using MvcRouteTester.AttributeRouting.Test.ApiControllers;
using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.ApiRoute
{
	[TestFixture]
	public class RouteMethodTests
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
				c.AddRoutesFromController<PostOnlyController>();
			});
		}

		[Test]
		public void CustomerControllerHasGetMethod()
		{
			RouteAssert.HasApiRoute(config, "/api/customer/1", HttpMethod.Get);
		}

		[Test]
		public void PostOnlyControllerHasPostMethod()
		{
			RouteAssert.HasApiRoute(config, "/api/postonly/1", HttpMethod.Post);
		}
	}
}
