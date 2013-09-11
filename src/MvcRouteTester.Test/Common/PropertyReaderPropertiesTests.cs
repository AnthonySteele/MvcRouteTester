using System;

using MvcRouteTester.Common;
using MvcRouteTester.Test.Controllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	[TestFixture]
	public class PropertyReaderPropertiesTests
	{
		private readonly PropertyReader reader = new PropertyReader();

		[Test]
		public void ShouldReadEmptyObject()
		{
			var properties = reader.Properties(new object());

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(0));
		}

		[Test]
		public void ShouldReadNothingFromNullObject()
		{
			var properties = reader.Properties(null);

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(0));
		}

		[Test]
		public void ShouldReadPropertiesOfAnonObject()
		{
			var properties = reader.Properties(new { Foo = 1, Bar = "Two" });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties.ValueByName("Foo"), Is.EqualTo(1));
			Assert.That(properties.ValueByName("Bar"), Is.EqualTo("Two"));
		}

		[Test]
		public void ShouldReadPropertiesOfStruct()
		{
			var values = new InputModelStruct
				{
					Id = 34,
					Name = "Bob"
				};
			var properties = reader.Properties(values);

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties.ValueByName("Id"), Is.EqualTo(34));
			Assert.That(properties.ValueByName("Name"), Is.EqualTo("Bob"));
		}

		[Test]
		public void ShouldReadPropertyValueNull()
		{
			var properties = reader.Properties(new { Foo = 1, Bar = (string)null });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties.ValueByName("Foo"), Is.EqualTo(1));
			Assert.That(properties.ValueByName("Bar"), Is.Null);
		}

		[Test]
		public void ShouldReadIntProperty()
		{
			var properties = reader.Properties(new { ValueUnderTest = 1 });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(1));
			Assert.That(properties.ValueByName("ValueUnderTest"), Is.EqualTo(1));
		}

		[Test]
		public void ShouldReadBoolProperty()
		{
			var properties = reader.Properties(new { ValueUnderTest = true });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(1));
			Assert.That(properties.ValueByName("ValueUnderTest"), Is.EqualTo(true));
		}

		[Test]
		public void ShouldReadStringProperty()
		{
			var properties = reader.Properties(new { ValueUnderTest = "Fish" });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(1));
			Assert.That(properties.ValueByName("ValueUnderTest"), Is.EqualTo("Fish"));
		}

		[Test]
		public void ShouldReadDecimalProperty()
		{
			var properties = reader.Properties(new { ValueUnderTest = 42.70m });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(1));
			Assert.That(properties.ValueByName("ValueUnderTest"), Is.EqualTo(42.7m));
		}

		[Test]
		public void ShouldReadGuidProperty()
		{
			var aGuid = Guid.NewGuid();
			var properties = reader.Properties(new { ValueUnderTest = aGuid });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(1));
			Assert.That(properties.ValueByName("ValueUnderTest"), Is.EqualTo(aGuid));
		}
	}
}
