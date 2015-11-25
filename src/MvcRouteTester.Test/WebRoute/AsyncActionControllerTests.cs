using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;
using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class AsyncActionControllerTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}

		[Test]
		public void HasRouteWorks()
		{
			var expectedRoute = new { controller = "AsyncAction", action = "IndexAsync" };
			RouteAssert.HasRoute(routes, "/AsyncAction/IndexAsync", expectedRoute);
		}

		[Test]
		public void ShouldCaptureId()
		{
			var expectedRoute = new { controller = "AsyncAction", action = "IndexAsync", Id = 42 };
			RouteAssert.HasRoute(routes, "/AsyncAction/IndexAsync/42", expectedRoute);
		}

		[Test]
		public void ShouldWorkWithFluentActionResult()
		{
			routes.ShouldMap("/AsyncAction/IndexAsync/42").To<AsyncActionController>(c => c.IndexAsync(42));
		}

        [Test]
        public void ShouldWorkWithFluentActionResultAndCancellationToken()
        {
            routes.ShouldMap("/AsyncAction/IndexWithCancellationAsync/42").To<AsyncActionController>(c => c.IndexWithCancellationAsync(42, CancellationToken.None));
        }

        [Test]
        public void ShouldWorkWithFluentJsonResult()
        {
            routes.ShouldMap("/AsyncAction/JsonAsync/42").To<AsyncActionController>(c => c.JsonAsync(42));
        }

        [Test]
        public void ShouldWorkWithFluentJsonResultAndCancellationToken()
        {
            routes.ShouldMap("/AsyncAction/JsonWithCancellationAsync/42").To<AsyncActionController>(c => c.JsonWithCancellationAsync(42, CancellationToken.None));
        }
    }
}
