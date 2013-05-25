using MvcRouteTester.Assertions;
using NUnit.Framework;

namespace MvcRouteTester.Test.Assertions
{
	public class NunitAssertEngine : IAssertEngine
	{
		public void Fail(string message)
		{
			Assert.Fail(message);
		}

		/// <summary>
		/// This could be a wrapper over "Fail" and reduce the size of the  IAssertEngine interface
		/// But then we would lose out on the error details that StringAssert.AreEqual provides
		/// </summary>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <param name="message"></param>
		public void StringsEqualIgnoringCase(string s1, string s2, string message)
		{
			if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
			{
				return;
			}

			StringAssert.AreEqualIgnoringCase(s1, s2, message);
		}
	}
}
