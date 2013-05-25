using System;
using System.Linq;
using MvcRouteTester.Common;
using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	public class DummyDerivedType: DummyClassType
	{
		public int Id2 { get; set; }
	}

	[TestFixture]
	public class PropertyReaderSimplePropertyNamesTests
	{
		private readonly PropertyReader reader = new PropertyReader();

		[Test]
		public void ShouldNotReadNullObject()
		{
			Assert.Throws<ArgumentNullException>(() => reader.SimplePropertyNames(null));
		}

		[Test]
		public void ShouldReadClassType()
		{
			var properties = reader.SimplePropertyNames(typeof(DummyClassType))
				.ToList();

			Assert.That(properties.Count, Is.EqualTo(2));
			Assert.That(properties.Contains("Id"), Is.True);
			Assert.That(properties.Contains("Name"), Is.True);
			Assert.That(properties.Contains("Foo"), Is.False);
		}

		[Test]
		public void ShouldNotReadBaseClassProperties()
		{
			var properties = reader.SimplePropertyNames(typeof(DummyDerivedType))
				.ToList();

			Assert.That(properties.Count, Is.EqualTo(1));
			Assert.That(properties.Contains("Id2"), Is.True);
		}
	}
}
