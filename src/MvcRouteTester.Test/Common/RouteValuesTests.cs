using MvcRouteTester.Common;

using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	[TestFixture]
	public class RouteValuesTests
	{
		[Test]
		public void TestCreate()
		{
			var values = new RouteValuesTests();
			Assert.That(values, Is.Not.Null);
		}

		[Test]
		public void TestRetrieveWithoutFromBodyFlag()
		{
			var values = RouteValuesContainingId();

			var valueOut = values.GetRouteValue("Id");

			Assert.That(valueOut, Is.Not.Null);
			Assert.That(valueOut.Name, Is.EqualTo("Id"));
			Assert.That(valueOut.Value, Is.EqualTo(42));
			Assert.That(valueOut.FromBody, Is.False);
		}

		[Test]
		public void TestRetrieveWithFromBodyFlag()
		{
			var values = RouteValuesContainingId();

			var valueOut = values.GetRouteValue("Id", false);

			Assert.That(valueOut, Is.Not.Null);
			Assert.That(valueOut.Name, Is.EqualTo("Id"));
			Assert.That(valueOut.Value, Is.EqualTo(42));
			Assert.That(valueOut.FromBody, Is.False);
		}

		[Test]
		public void TestRetrieveFailsWithFromBodyFlagNotMatching()
		{
			var values = RouteValuesContainingId();

			var valueOut = values.GetRouteValue("Id", true);

			Assert.That(valueOut, Is.Null);
		}

		private static RouteValues RouteValuesContainingId()
		{
			var values = new RouteValues();
			var idValue = new RouteValue
				{
					Name = "Id", 
					Value = 42, 
					FromBody = false
				};
			values.Add(idValue);

			return values;
		}
	}
}
