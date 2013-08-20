using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class GeneratesUrlFailTests
	{
		private RouteCollection routes;
		private FakeAssertEngine assertEngine;

		[SetUp]
		public void MakeRouteTable()
		{
			assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);

			routes = new RouteCollection();
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void Fail_message_on_action_is_as_expected()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", "NoSuchAction", "Home");

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Generated url does not equal to expected url. Generated: '/Home/NoSuchAction', expected: '/'"));
		}

		[Test]
		public void Fail_message_on_controller_is_as_expected()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", "Index", "NoSuchController");

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Generated url does not equal to expected url. Generated: '/NoSuchController', expected: '/'"));
		}

		[Test]
		public void Fail_message_is_as_expected_with_anon_object()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", new { action = "NoSuchAction", controller = "Home" });

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Generated url does not equal to expected url. Generated: '/Home/NoSuchAction', expected: '/'"));
		}

		[Test]
		public void Fail_message_is_as_expected_with_dictionary()
		{
			var values = new Dictionary<string,string>
				{
					{ "action", "NoSuchAction"},
					{ "controller", "Home"},
				};

			RouteAssert.GeneratesActionUrl(routes, "/", values);

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Generated url does not equal to expected url. Generated: '/Home/NoSuchAction', expected: '/'"));
		}

		[Test]
		public void Fail_message_on_action_is_as_expected_with_current_url()
		{
			RouteAssert.GeneratesActionUrl(routes, "/", "NoSuchAction", "Home", HttpMethod.Get, "/foo");

			Assert.That(assertEngine.FailCount, Is.EqualTo(1));
			Assert.That(assertEngine.Messages[0], Is.EqualTo("Generated url does not equal to expected url. Generated: '/foo/Home/NoSuchAction', expected: '/'"));
		}
	}
}
