using System.Net.Http;
using System.Web.Http;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class RouteMethodTests
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
		public void CustomerControllerHasGetMethod()
		{
			RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get);
		}

		[Test]
		public void CustomerControllerDoesNotHavePostMethod()
		{
			RouteAssert.ApiRouteDoesNotHaveMethod(config, "~/api/customer/1", HttpMethod.Post);
		}

		[Test]
		public void PostOnlyControllerHasPostMethod()
		{
			RouteAssert.HasApiRoute(config, "~/api/postonly/1", HttpMethod.Post);
		}

		[Test]
		public void PostOnlyControllerDoesNotHaveGetMethod()
		{
			RouteAssert.ApiRouteDoesNotHaveMethod(config, "~/api/postonly/1", HttpMethod.Get);
		}
	}
}
