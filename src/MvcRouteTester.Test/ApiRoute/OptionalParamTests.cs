using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class OptionalParamTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void Setup()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			config = new HttpConfiguration();

			config.Routes.MapHttpRoute(
				name: "OptionalParamApi",
				routeTemplate: "api/{controller}/{paramA}",
				defaults: new { paramB = RouteParameter.Optional });
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamSupplied()
		{
			var expectations = new
				{
					controller = "WithOptionalParam",
					action = "Get",
					paramA = 42,
					paramB = 34
				};

			RouteAssert.HasApiRoute(config, "/api/WithOptionalParam/42?paramB=34", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamSuppliedWithFluentExpectation()
		{
			config.ShouldMap("/api/WithOptionalParam/42?paramB=34")
				.To<WithOptionalParamController>(HttpMethod.Get, x => x.Get(42, 34));
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamNotSupplied()
		{
			var expectations = new
			{
				controller = "WithOptionalParam",
				action = "Get",
				paramA = 42,
			};

			RouteAssert.HasApiRoute(config, "/api/WithOptionalParam/42/", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamNotSuppliedAndExpectationOfNull()
		{
			var expectations = new
				{
					controller = "WithOptionalParam",
					action = "Get",
					paramA = 42,
					paramB = (int?)null
				};

			RouteAssert.HasApiRoute(config, "/api/WithOptionalParam/42/", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamNotSuppliedFluentExpectation()
		{
			config.ShouldMap("/api/WithOptionalParam/42/")
				.To<WithOptionalParamController>(HttpMethod.Get, x => x.Get(42, null));
		}
	}
}
