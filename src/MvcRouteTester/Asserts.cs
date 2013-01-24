using NUnit.Framework;

namespace MvcRouteTester
{
	public static class Asserts
	{
		public static void Fail(string message)
		{
			Assert.Fail(message);
		}
		
		public static void StringsEqualIgnoringCase(string s1, string s2, string message)
		{
			StringAssert.AreEqualIgnoringCase(s1, s2, message);
		}
	}
}
