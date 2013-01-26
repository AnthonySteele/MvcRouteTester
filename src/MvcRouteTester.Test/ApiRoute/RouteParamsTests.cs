using System.Net.Http;
using System.Web.Http;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class RouteParamsTests
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
		public void HasRouteWithParams()
		{
			RouteAssert.HasApiRoute(config, "/api/customer/1?foo=1&bar=2", HttpMethod.Get);
		}

		[Test]
		public void HasRouteWithParamsCapturesValues()
		{
			var expectedRoute = new { controller = "customer", action = "get", id = "1", foo = "1", bar = "2" };
			RouteAssert.HasApiRoute(config, "/api/customer/1?foo=1&bar=2", HttpMethod.Get, expectedRoute);
		}
	}
}
