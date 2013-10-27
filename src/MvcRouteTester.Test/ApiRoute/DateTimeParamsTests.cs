using System;
using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class DateTimeParamsTests
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
		public void WorksWithoutIdTested()
		{
			var expectedRoute = new { controller = "WithDateTime", action = "Get" };
			RouteAssert.HasApiRoute(config, "/api/WithDateTime", HttpMethod.Get, expectedRoute);
		}

		[Test]
		public void ValueIsCaptured()
		{
			var expectedRoute = new { controller = "WithDateTime", action = "Get", id = new DateTime(2012, 5, 30) };
			RouteAssert.HasApiRoute(config, "/api/WithDateTime/2012-05-30", HttpMethod.Get, expectedRoute);
		}

		[Test]
		public void DifferentValueIsError()
		{
			var asserts = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(asserts);
			var expectedRoute = new { controller = "WithDateTime", action = "Get", id = new DateTime(2012, 5, 30) };

			RouteAssert.HasApiRoute(config, "/api/WithDateTime/2013-6-28", HttpMethod.Get, expectedRoute);

			Assert.That(asserts.FailCount, Is.EqualTo(1));
			Assert.That(asserts.Messages[0], Is.EqualTo("Expected '2012-05-30T00:00:00', not '2013-6-28' for 'id' at url '/api/WithDateTime/2013-6-28'."));
		}
	}
}
