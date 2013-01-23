using NUnit.Framework;

namespace MvcRouteTester.Test
{
	[TestFixture]
	public class AssertsTests
	{
		[Test]
		public void IsNullPasses()
		{
			Asserts.Null(null, "fail");
		}

		[Test]
		public void NotNullPasses()
		{
			Asserts.NotNull(new object(), "fail");
		}

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
	}
}
