using System;
using System.Collections.Generic;
using MvcRouteTester.Common;
using MvcRouteTester.Test.ApiControllers;
using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	public class DummyClassType
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	[TestFixture]
	public class PropertyReaderIsSimpleTypeTests
	{
		private readonly PropertyReader reader = new PropertyReader();

		[Test]
		public void IntIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(int)), Is.True);
		}

		[Test]
		public void StringIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(string)), Is.True);
		}

		[Test]
		public void DoubleIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(double)), Is.True);
		}

		[Test]
		public void DecimalIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(decimal)), Is.True);
		}

		[Test]
		public void BoolIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(bool)), Is.True);
		}

		[Test]
		public void DateTimeIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(DateTime)), Is.True);
		}

		[Test]
		public void ClassIsNotSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(PropertyReaderIsSimpleTypeTests)), Is.False);
		}

		[Test]
		public void DTOIsNotSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(DummyClassType)), Is.False);
		}

		[Test]
		public void ListIsNotSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(List<string>)), Is.False);
		}

		[Test]
		public void AnonObjectIsNotSimpleType()
		{
			var dummyObject = new { id = 1, Name = "fred" };
			Assert.That(reader.IsSimpleType(dummyObject.GetType()), Is.False);
		}

		[Test]
		public void NullableIntIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(int?)), Is.True);
		}

		[Test]
		public void SimpleInputModelIsSimpleType()
		{
			Assert.That(reader.IsSimpleType(typeof(SimpleInputModel)), Is.True);
		}
	}
}
