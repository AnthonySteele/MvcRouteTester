using System;

using MvcRouteTester.Common;
using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	[TestFixture]
	public class PropertyReaderIgnoreAttributesTests
	{
		private readonly PropertyReader reader = new PropertyReader();

		[Test]
		public void ShouldIgnorePropertiesWithSpecifiedAttributes()
		{
			RouteAssert.AddIgnoreAttributes(new[]
				{
					typeof (IgnoreMeAttribute)
				});

			var propertiesList = reader.PropertiesList(new TestStub());

			Assert.That(propertiesList.Count, Is.EqualTo(1));
			Assert.That(propertiesList[0].Name, Is.EqualTo("NotIgnored"));
		}

		private class IgnoreMeAttribute : Attribute
		{
		}

		private class TestStub
		{
			[IgnoreMe]
			public string Ignored { get; set; }

			public string NotIgnored { get; set; }
		}
	}
}
