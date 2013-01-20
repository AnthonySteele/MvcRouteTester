using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class RouteExpectionTests
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
		public void HasApiRouteWithExpectation()
		{
			var expectations = new { controller = "Customer", action= "get", id = "1" };
			RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get, expectations);
		}

		[Test]
		public void HasApiRouteWithControllerAndActionParams()
		{
			RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get, "customer", "get");
		}

		[Test]
		public void HasApiRouteWithExpectionasInDictionary()
		{
			var expectations = new Dictionary<string, string>
				{
					{ "controller", "Customer" },
					{ "action", "Get" },
					{ "id", "1" }
				};
			
			RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get, expectations);
		}

		[Test]
		public void HasApiRouteWithExpectationOnPost()
		{
			var expectations = new { controller = "PostOnly", action = "Post", id = "1" };
			RouteAssert.HasApiRoute(config, "~/api/postonly/1", HttpMethod.Post, expectations);
		}

		[Test]
		public void ShouldMatchActionNameToMethodName()
		{
			var expectations = new { controller = "Renamed", action = "GetWithADifferentName", id = "1" };
			RouteAssert.HasApiRoute(config, "~/api/renamed/1", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldNotFindNonexistentRoute()
		{
			RouteAssert.NoApiRoute(config, "~/pai/customer/1");
		}
	}
}
