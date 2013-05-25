using MvcRouteTester.Assertions;
using NUnit.Framework;
using AssertionException = MvcRouteTester.Assertions.AssertionException;

namespace MvcRouteTester.Test.Assertions
{
	[TestFixture]
	public class ExceptionAssertEngineTests
	{
		[Test]
		public void FailThrowsException()
		{
			var assertEngine = new ExceptionAssertEngine();
			Assert.Throws<AssertionException>(() => assertEngine.Fail("Failed"));
		}

		[Test]
		public void StringMismatchThrowsException()
		{
			var assertEngine = new ExceptionAssertEngine();
			Assert.Throws<AssertionException>(() => assertEngine.StringsEqualIgnoringCase("foo", "bar", "Failed"));
		}

		[Test]
		public void StringMatchDoesNotThrowException()
		{
			var assertEngine = new ExceptionAssertEngine();
			Assert.DoesNotThrow(() => assertEngine.StringsEqualIgnoringCase("foo", "foo", "Failed"));
		}

		[Test]
		public void EmptyAndNullStringsDoesNotThrowException()
		{
			var assertEngine = new ExceptionAssertEngine();
			Assert.DoesNotThrow(() => assertEngine.StringsEqualIgnoringCase(string.Empty, null, "Failed"));
		}
	}
}
