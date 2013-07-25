using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class WithOptionalParamTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			routes = new RouteCollection();
			routes.MapRoute(
				"CountryId",
				"{country}/withparams/{action}/{id}",
				new { controller = "WithOptionalParam", country = "uk", action = "Search", id = UrlParameter.Optional });
		}

		[Test]
		public void HasRouteWhenOptionalParamIsSpecified()
		{
			RouteAssert.HasRoute(routes, "/uk/withparams/Search/123");
		}

		[Test]
		public void MatchesRouteWhenOptionalParamIsSpecified()
		{
			RouteAssert.HasRoute(routes, "/uk/withparams/Search/123", new
				{
					Controller = "WithOptionalParam",
					Action = "Search",
					Id = "123"
				});
		}

		[Test]
		public void HasRouteWhenOptionalParamIsNotSpecified()
		{
			RouteAssert.HasRoute(routes, "/uk/withparams/Search/");
		}

		[Test]
		public void MatchesRouteWhenOptionalParamIsNotSpecified()
		{
			RouteAssert.HasRoute(routes, "/uk/withparams/Search/", new
			{
				Controller = "WithOptionalParam",
				Action = "Search"
			});
		}

	}
}
