using System;

namespace MvcRouteTester.Assertions
{
	public class ExceptionAssertEngine : IAssertEngine
	{
		public void Fail(string message)
		{
			throw new AssertionException(message);
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

			if (string.Equals(s1, s2, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			var exceptionMessage = string.Format("{0}\n Strings do not match: \n Expected:{1}\nActual:{2}", message, s1, s2);
			throw new AssertionException(exceptionMessage);
		}
	}
}
