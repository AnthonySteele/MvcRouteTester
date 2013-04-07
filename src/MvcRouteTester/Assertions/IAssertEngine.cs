namespace MvcRouteTester.Assertions
{
	public interface IAssertEngine
	{
		void Fail(string message);
		void StringsEqualIgnoringCase(string s1, string s2, string message);
	}
}