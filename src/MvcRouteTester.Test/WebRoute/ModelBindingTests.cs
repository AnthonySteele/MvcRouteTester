using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class ModelBindingTests
	{
		private RouteCollection routes;

		[SetUp]
		public void MakeRouteTable()
		{
			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}/{name}",
				defaults: new { controller = "Home", action = "Index" });
		}

		[Test]
		public void HasRoute()
		{
			RouteAssert.HasRoute(routes, "/withobject/index/1/fred");
		}

		[Test]
		public void HasRouteToController()
		{
			var expectedRoute = new
			{
				controller = "withobject",
				action = "Index"
			};

			RouteAssert.HasRoute(routes, "/withobject/index/1/fred", expectedRoute);
		}
	}
}
