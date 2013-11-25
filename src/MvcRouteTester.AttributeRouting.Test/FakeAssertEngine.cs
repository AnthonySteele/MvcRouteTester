using System;
using System.Collections.Generic;

using MvcRouteTester.Assertions;

namespace MvcRouteTester.AttributeRouting.Test
{
	class FakeAssertEngine : IAssertEngine
	{
		private readonly IList<string> messages = new List<string>();

		public int FailCount { get; set; }
		public int StringMismatchCount { get; set; }
		public int StringMatchCount { get; set; }

		public IList<string> Messages
		{
			get { return messages; }
		}

		public void Fail(string message)
		{
			FailCount++;
			messages.Add(message);
		}

		public void StringsEqualIgnoringCase(string s1, string s2, string message)
		{
			if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
			{
				StringMatchCount++;
				return;
			}

			if (string.Equals(s1, s2, StringComparison.InvariantCultureIgnoreCase))
			{
				StringMatchCount++;
			}
			else
			{
				StringMismatchCount++;
				messages.Add(message);
			}
		}
	}
}
