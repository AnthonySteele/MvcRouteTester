using System.Collections.Generic;

using NUnit.Framework;

namespace MvcRouteTester.Test
{
	[TestFixture]
	public class PropertyReaderIsSimpleTypeTests
	{
		private readonly PropertyReader propertyReader = new PropertyReader();

		[Test]
		public void IntIsSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(int)), Is.True);
		}

		[Test]
		public void StringIsSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(string)), Is.True);
		}

		[Test]
		public void DoubleIsSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(double)), Is.True);
		}

		[Test]
		public void DecimalIsSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(decimal)), Is.True);
		}

		[Test]
		public void BoolIsSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(bool)), Is.True);
		}

		[Test]
		public void ClassIsNotSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(PropertyReaderIsSimpleTypeTests)), Is.False);
		}

		[Test]
		public void ListIsNotSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(List<string>)), Is.False);
		}

		[Test]
		public void AnonObjectIsNotSimpleType()
		{
			var dummyObject = new { id = 1, Name = "fred" };
			Assert.That(propertyReader.IsSimpleType(dummyObject.GetType()), Is.False);
		}

		[Test]
		public void NullableIntIsSimpleType()
		{
			Assert.That(propertyReader.IsSimpleType(typeof(int?)), Is.True);
		}

	}
}
