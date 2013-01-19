using System;

using NUnit.Framework;

namespace MvcRouteTester.Test
{
	[TestFixture]
	public class PropertyReaderTests
	{
		[Test]
		public void ShouldReadEmptyObject()
		{
			var reader = new PropertyReader();
			var properties = reader.Properties(new object());

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(0));
		}

		[Test]
		public void ShouldNotReadNullObject()
		{
			var reader = new PropertyReader();

			Assert.Throws<ArgumentNullException>(() => reader.Properties(null));
		}

		[Test]
		public void ShouldReadPropertiesOfAnonObject()
		{
			var reader = new PropertyReader();
			var properties = reader.Properties(new { Foo = 1, Bar = "Two" });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties["Foo"], Is.EqualTo("1"));
			Assert.That(properties["Bar"], Is.EqualTo("Two"));
		}

		[Test]
		public void ShouldReadPropertyValueNull()
		{
			var reader = new PropertyReader();
			var properties = reader.Properties(new { Foo = 1, Bar = (string)null });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties["Foo"], Is.EqualTo("1"));
			Assert.That(properties["Bar"], Is.Null);
		}
	}
}
