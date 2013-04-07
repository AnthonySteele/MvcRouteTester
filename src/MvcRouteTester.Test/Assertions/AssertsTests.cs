using MvcRouteTester.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.Assertions
{
	[TestFixture]
	public class AssertsTests
	{
		[Test]
		public void EqualStringsPass()
		{
			Asserts.StringsEqualIgnoringCase("foo", "foo", "fail");
		}

		[Test]
		public void DifferentCaseStringsPass()
		{
			Asserts.StringsEqualIgnoringCase("foo", "Foo", "fail");
		}

		[Test]
		public void NullStringsAreEqual()
		{
			Asserts.StringsEqualIgnoringCase(null, null, "fail");
		}

		[Test]
		public void EmptyStringIsSameAsNull()
		{
			Asserts.StringsEqualIgnoringCase(string.Empty, null, "fail");
		}
	}
}
