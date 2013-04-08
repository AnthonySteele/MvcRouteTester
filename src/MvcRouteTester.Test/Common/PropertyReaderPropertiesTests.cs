using System;
using MvcRouteTester.Common;
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
		public void ShouldNotReadNullObject()
		{
			Assert.Throws<ArgumentNullException>(() => reader.Properties(null));
		}

		[Test]
		public void ShouldReadPropertiesOfAnonObject()
		{
			var properties = reader.Properties(new { Foo = 1, Bar = "Two" });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties["Foo"], Is.EqualTo("1"));
			Assert.That(properties["Bar"], Is.EqualTo("Two"));
		}

		[Test]
		public void ShouldReadPropertyValueNull()
		{
			var properties = reader.Properties(new { Foo = 1, Bar = (string)null });

			Assert.That(properties, Is.Not.Null);
			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties["Foo"], Is.EqualTo("1"));
			Assert.That(properties["Bar"], Is.Null);
		}
	}
}
