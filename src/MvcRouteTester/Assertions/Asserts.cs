namespace MvcRouteTester.Assertions
{
	public static class Asserts
	{
		static Asserts()
		{
			AssertEngineEngine = new NunitAssertEngine();
		}

		public static IAssertEngine AssertEngineEngine { get; set; }

		public static void Fail(string message)
		{
			AssertEngineEngine.Fail(message);
		}

		public static void StringsEqualIgnoringCase(string s1, string s2, string message)
		{
			AssertEngineEngine.StringsEqualIgnoringCase(s1, s2, message);
		}
	}
}
