namespace MvcRouteTester.Assertions
{
	public static class Asserts
	{
		static Asserts()
		{
			AssertEngine = new NunitAssertEngine();
		}

		internal static IAssertEngine AssertEngine { get; set; }

		public static void Fail(string message)
		{
			AssertEngine.Fail(message);
		}

		public static void StringsEqualIgnoringCase(string s1, string s2, string message)
		{
			AssertEngine.StringsEqualIgnoringCase(s1, s2, message);
		}
	}
}
