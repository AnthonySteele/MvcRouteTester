using System.Net.Http;
using System.Threading;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class AsyncActionControllerTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			config = new HttpConfiguration();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional });
		}

		[Test]
		public void RecognisesSimpleRoute()
		{
			var expectedRoute = new { controller = "AsyncAction", action = "GetAsync" };
			RouteAssert.HasApiRoute(config, "/api/AsyncAction", HttpMethod.Get, expectedRoute);
		}

        [Test]
        public void RecognisesSimpleRouteAndCancellationToken()
        {
            var expectedRoute = new { controller = "AsyncAction", action = "PutWithCancellationAsync" };
            RouteAssert.HasApiRoute(config, "/api/AsyncAction", HttpMethod.Put, expectedRoute);
        }

        [Test]
		public void RecognisesRouteWithParams()
		{
			var expectedRoute = new { controller = "AsyncAction", action = "GetAsync" };
			RouteAssert.HasApiRoute(config, "/api/AsyncAction/123", HttpMethod.Get, expectedRoute);
		}

        [Test]
        public void RecognisesRouteWithParamsAndCancellationToken()
        {
            var expectedRoute = new { controller = "AsyncAction", action = "PutWithCancellationAsync" };
            RouteAssert.HasApiRoute(config, "/api/AsyncAction/123", HttpMethod.Put, expectedRoute);
        }

        [Test]
		public void RecognisesFluentRoute()
		{
			config.ShouldMap("/api/AsyncAction/123")
				.To<AsyncActionController>(HttpMethod.Get, x => x.GetAsync(123));
		}

        [Test]
        public void RecognisesFluentRouteWithCancelllationToken()
        {
            config.ShouldMap("/api/AsyncAction/40")
                .To<AsyncActionController>(HttpMethod.Put, x => x.PutWithCancellationAsync(40, CancellationToken.None));
        }
    }
}
