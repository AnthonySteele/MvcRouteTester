using MvcRouteTester.ApiRoute;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	[TestFixture]
	public class FormUrlBodyReaderTests
	{
		[Test]
		public void CanReadEmptyBody()
		{
			var reader = new FormUrlBodyReader();
			var result = reader.ReadBody(string.Empty);
			
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.Empty);
		}

		[Test]
		public void CanReadSingleValue()
		{
			var reader = new FormUrlBodyReader();
			var result = reader.ReadBody("id=42");

			Assert.That(result.Count, Is.EqualTo(1));
			var value = result[0];
			Assert.That(value.Name, Is.EqualTo("id"));
			Assert.That(value.Value, Is.EqualTo("42"));
		}

		[Test]
		public void CanReadTwoValues()
		{
			var reader = new FormUrlBodyReader();
			var result = reader.ReadBody("id=42&name=fred");

			Assert.That(result.Count, Is.EqualTo(2));
			var value1 = result[0];
			Assert.That(value1.Name, Is.EqualTo("id"));
			Assert.That(value1.Value, Is.EqualTo("42"));

			var value2 = result[1];
			Assert.That(value2.Name, Is.EqualTo("name"));
			Assert.That(value2.Value, Is.EqualTo("fred"));
		}

		[Test]
		public void CanReadMultipleValues()
		{
			var reader = new FormUrlBodyReader();
			var result = reader.ReadBody("val1=a&val2=b&val3=c&val4=d&val5=5");

			Assert.That(result.Count, Is.EqualTo(5));
		}
	}
}
