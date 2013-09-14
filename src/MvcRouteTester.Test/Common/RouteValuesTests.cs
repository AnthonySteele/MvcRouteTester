using System.Collections.Generic;
using System.Web.Routing;

using MvcRouteTester.Common;

using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	[TestFixture]
	public class RouteValuesTests
	{
		private FakeAssertEngine assertEngine;

		[SetUp]
		public void Setup()
		{
			assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
		}

		[Test]
		public void TestCreate()
		{
			var values = new RouteValues();
			Assert.That(values, Is.Not.Null);
		}

		[Test]
		public void TestCreateWithStringDictionary()
		{
			var valuesIn = new Dictionary<string, string>
				{
					 { "controller", "foo" },
					 { "action", "bar" },
					 { "area", "fish" },
					 { "Id", "3" },
				};

			var values = new RouteValues(valuesIn);

			Assert.That(values, Is.Not.Null);
			Assert.That(values.Controller, Is.EqualTo("foo"));
			Assert.That(values.Action, Is.EqualTo("bar"));
			Assert.That(values.Area, Is.EqualTo("fish"));

			Assert.That(values.GetRouteValue("Id", RouteValueOrigin.Unknown).Value, Is.EqualTo("3"));
		}

		[Test]
		public void TestCreateWithObjectDictionary()
		{
			var valuesIn = new Dictionary<string, object>
				{
					 { "controller", "foo" },
					 { "action", "bar" },
					 { "area", "fish" },
					 { "Id", 3 },
				};

			var values = new RouteValues(valuesIn);

			Assert.That(values, Is.Not.Null);
			Assert.That(values.Controller, Is.EqualTo("foo"));
			Assert.That(values.Action, Is.EqualTo("bar"));
			Assert.That(values.Area, Is.EqualTo("fish"));

			Assert.That(values.GetRouteValue("Id", RouteValueOrigin.Unknown).Value, Is.EqualTo(3));
		}

		[Test]
		public void ReadsEmptyDataAsFail()
		{
			var data = new Dictionary<string, string>();
			var props = new RouteValues(data);
			props.CheckDataOk();

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

			var props = new RouteValues(data);
			props.CheckDataOk();

			Assert.That(props.DataOk, Is.False, "data ok");
		}

		[Test]
		public void ReadsNoControllerDataAsFail()
		{
			var data = new Dictionary<string, string>
				{
					{ "action", "foo" }
				};

			var props = new RouteValues(data);
			props.CheckDataOk();

			Assert.That(props.DataOk, Is.False, "data ok");
		}

		[Test]
		public void ReadsControllerAndActionDataAsOk()
		{
			var data = new Dictionary<string, string>
				{
					{ "controller", "foo" },
					{ "action", "bar" }
				};

			var props = new RouteValues(data);
			props.CheckDataOk();

			Assert.That(props.DataOk, Is.True, "data ok");
		}

		[Test]
		public void FailsAreReported()
		{
			var data = new Dictionary<string, string>();
			var routeValues = new RouteValues(data);
			routeValues.CheckDataOk();

			Assert.That(assertEngine.FailCount, Is.EqualTo(2));
			Assert.That(routeValues.DataOk, Is.False);
		}

		[Test]
		public void ReadRouteValueDictionary()
		{
			var data = new RouteValueDictionary
				{
					{ "controller", "foo" },
					{ "action", "bar" },
					{ "id", 3 },
				};

			var values = new RouteValues(data);

			Assert.That(values, Is.Not.Null);
			Assert.That(values.Controller, Is.EqualTo("foo"));
			Assert.That(values.Action, Is.EqualTo("bar"));

			Assert.That(values.GetRouteValue("Id", RouteValueOrigin.Unknown).Value, Is.EqualTo(3));
		}

		[Test]
		public void ReadsEmptyDataWithoutRouteValues()
		{
			var data = new Dictionary<string, string>();
			var props = new RouteValues(data);

			Assert.That(props.Values, Is.Not.Null, "route values null");
			Assert.That(props.Values.Count, Is.EqualTo(0), "route values empty");
		}

		[Test]
		public void ReadsControllerAndAction()
		{
			var data = new Dictionary<string, string>
				{
					{ "controller", "foo" },
					{ "action", "bar" },
				};

			var props = new RouteValues(data);

			Assert.That(props.Controller, Is.EqualTo("foo"), "controller");
			Assert.That(props.Action, Is.EqualTo("bar"), "action");
			Assert.That(props.Values.Count, Is.EqualTo(0), "route values empty");
		}

		[Test]
		public void ReadsControllerAndActionToDataOk()
		{
			var data = new Dictionary<string, string>
				{
					{ "controller", "foo" },
					{ "action", "bar" },
				};

			var props = new RouteValues(data);
			props.CheckDataOk();

			Assert.That(props.DataOk, Is.True, "data ok");
		}

		[Test]
		public void AsRouteValueDictionaryReadsAllValues()
		{
			var data = new Dictionary<string, object>
				{
					{ "id", 3 }
				};

			var values = new RouteValues(data);
			values.Controller = "foo";
			values.Action = "bar";
			values.Area = "fish";

			var outValues = values.AsRouteValueDictionary();

			Assert.That(outValues.Count, Is.EqualTo(4));
			Assert.That(outValues["controller"], Is.EqualTo("foo"));
			Assert.That(outValues["action"], Is.EqualTo("bar"));
			Assert.That(outValues["area"], Is.EqualTo("fish"));
			Assert.That(outValues["id"], Is.EqualTo(3));
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

			var props = new RouteValues(data);
			props.CheckDataOk();

			Assert.That(props.DataOk, Is.True, "data ok");
			Assert.That(props.Values.Count, Is.EqualTo(1), "route values not empty");

			var output = props.GetRouteValue("fish", RouteValueOrigin.Unknown);

			Assert.That(output, Is.Not.Null, "route value missing");
			Assert.That(output.ValueAsString, Is.EqualTo("hallibut"), "route value wrong");
		}


		[Test]
		public void TestRetrieveWithFromBodyFlagMatching()
		{
			var values = RouteValuesContainingId();

			var valueOut = values.GetRouteValue("Id", RouteValueOrigin.Unknown);

			Assert.That(valueOut, Is.Not.Null);
			Assert.That(valueOut.Name, Is.EqualTo("Id"));
			Assert.That(valueOut.Value, Is.EqualTo(42));
			Assert.That(valueOut.Origin, Is.EqualTo(RouteValueOrigin.Unknown));
		}

		[Test]
		public void TestRetrieveWithDifferentCase()
		{
			var values = RouteValuesContainingId();

			var valueOut = values.GetRouteValue("id", RouteValueOrigin.Unknown);

			Assert.That(valueOut, Is.Not.Null);
			Assert.That(valueOut.Name, Is.EqualTo("Id"));
			Assert.That(valueOut.Value, Is.EqualTo(42));
			Assert.That(valueOut.Origin, Is.EqualTo(RouteValueOrigin.Unknown));
		}

		[Test]
		public void TestRetrieveFailsWithFromBodyFlagNotMatching()
		{
			var values = RouteValuesContainingId();

			var valueOut = values.GetRouteValue("Id", RouteValueOrigin.Body);

			Assert.That(valueOut, Is.Null);
		}

		private static RouteValues RouteValuesContainingId()
		{
			var values = new RouteValues();
			var idValue = new RouteValue("Id", 42, RouteValueOrigin.Unknown);
			values.Add(idValue);

			return values;
		}
	}
}
