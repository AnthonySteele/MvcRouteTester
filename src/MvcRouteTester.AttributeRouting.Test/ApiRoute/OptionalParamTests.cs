using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.AttributeRouting.Test.ApiControllers;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.ApiRoute
{
	[TestFixture]
	public class OptionalParamTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.EnsureInitialized();
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamSupplied()
		{
			var expectations = new
				{
					controller = "WithOptionalParamAttr",
					action = "Get",
					paramA = 42,
					paramB = 34
				};

			RouteAssert.HasApiRoute(config, "/someapi/withoptionalparam/42?paramB=34", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamSuppliedWithFluentExpectation()
		{
			config.ShouldMap("/someapi/withoptionalparam/42?paramB=34")
				.To<WithOptionalParamAttrController>(HttpMethod.Get, x => x.Get(42, 34));
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamNotSupplied()
		{
			var expectations = new
			{
				controller = "WithOptionalParamAttr",
				action = "Get",
				paramA = 42,
			};

			RouteAssert.HasApiRoute(config, "/someapi/withoptionalparam/42/", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamNotSuppliedAndExpectationOfNull()
		{
			var expectations = new
				{
					controller = "WithOptionalParamAttr",
					action = "Get",
					paramA = 42,
					paramB = (int?)null
				};

			RouteAssert.HasApiRoute(config, "/someapi/withoptionalparam/42/", HttpMethod.Get, expectations);
		}

		[Test]
		public void ShouldHaveRouteWithOptionalParamNotSuppliedFluentExpectation()
		{
			config.ShouldMap("/someapi/withoptionalparam/42/")
				.To<WithOptionalParamAttrController>(HttpMethod.Get, x => x.Get(42, null));
		}
	}
}
