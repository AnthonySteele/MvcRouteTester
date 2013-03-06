using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class NullableParamTests
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
		public void WorksWithoutIdTested()
		{
			var expectedRoute = new { controller = "WithNullable", action = "Get" };
			RouteAssert.HasApiRoute(config, "/api/WithNullable", HttpMethod.Get, expectedRoute);
		}

		[Test]
		public void NullValueIsCaptured()
		{
			var expectedRoute = new { controller = "WithNullable", action = "Get", id = (int?)null };
			RouteAssert.HasApiRoute(config, "/api/WithNullable", HttpMethod.Get, expectedRoute);
		}

		[Test]
		public void NonNullValueIsCaptured()
		{
			var expectedRoute = new { controller = "WithNullable", action = "Get", id = 47 };
			RouteAssert.HasApiRoute(config, "/api/WithNullable?id=47", HttpMethod.Get, expectedRoute);
		}

		[Test]
		public void FluentMapToNull()
		{
			config.ShouldMap("/api/WithNullable").To<WithNullableController>(HttpMethod.Get, x => x.Get(null));
		}

		[Test]
		public void FluentMapToNotNull()
		{
			config.ShouldMap("/api/WithNullable?id=47").To<WithNullableController>(HttpMethod.Get, x => x.Get(47));
		}
	}
}
