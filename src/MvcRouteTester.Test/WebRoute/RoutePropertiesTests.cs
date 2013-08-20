using System.Collections.Generic;

using MvcRouteTester.Test.Assertions;
using MvcRouteTester.WebRoute;

using NUnit.Framework;

namespace MvcRouteTester.Test.WebRoute
{
	[TestFixture]
	public class RoutePropertiesTests
	{
		private FakeAssertEngine assertEngine;

		[SetUp]
		public void MakeRouteTable()
		{
			assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
		}

		[TearDown]
		public void TearDown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}

		[Test]
		public void ReadsEmptyDataAsFail()
		{
			var data = new Dictionary<string, string>();
			var props = new RouteProperties(data);

			Assert.That(props.DataOk, Is.False, "data ok");
			Assert.That(props.Controller, Is.Null, "controller");
			Assert.That(props.Action, Is.Null, "action");
		}

		[Test]
		public void ReadsNoActionDataAsFail()
		{
			var data = new Dictionary<string, string>
				{
					{ "controller", "foo" }
				};
			
			var props = new RouteProperties(data);

			Assert.That(props.DataOk, Is.False, "data ok");
		}

		[Test]
		public void ReadsNoControllerDataAsFail()
		{
			var data = new Dictionary<string, string>
				{
					{ "action", "foo" }
				};

			var props = new RouteProperties(data);

			Assert.That(props.DataOk, Is.False, "data ok");
		}

		[Test]
		public void FailsAReReported()
		{
			var data = new Dictionary<string, string>();
			new RouteProperties(data);

			Assert.That(assertEngine.FailCount, Is.EqualTo(2));
		}

		[Test]
		public void ReadsEmptyDataWithoutRouteValues()
		{
			var data = new Dictionary<string, string>();
			var props = new RouteProperties(data);

			Assert.That(props.RouteValues, Is.Not.Null, "route values null");
			Assert.That(props.RouteValues.Count, Is.EqualTo(0), "route values empty");
		}

		[Test]
		public void ReadsControllerAndAction()
		{
			var data = new Dictionary<string, string>
				{
					{ "controller", "foo" },
					{ "action", "bar" },
				};
			var props = new RouteProperties(data);

			Assert.That(props.DataOk, Is.True, "data ok");
			Assert.That(props.Controller, Is.EqualTo("foo"), "controller");
			Assert.That(props.Action, Is.EqualTo("bar"), "action");
			Assert.That(props.RouteValues.Count, Is.EqualTo(0), "route values empty");
		}

		[Test]
		public void ReadsOtherValuesAsRouteValues()
		{
			var data = new Dictionary<string, string>
				{
					{ "controller", "foo" },
					{ "action", "bar" },
					{ "fish", "hallibut" },
				};
			var props = new RouteProperties(data);

			Assert.That(props.DataOk, Is.True, "data ok");
			Assert.That(props.RouteValues.Count, Is.EqualTo(1), "route values not empty");
			Assert.That(props.RouteValues["fish"], Is.EqualTo("hallibut"), "route value wrong");
		}
	}
}
