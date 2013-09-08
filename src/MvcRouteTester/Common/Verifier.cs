using System;
using System.Collections.Generic;
using MvcRouteTester.Assertions;

namespace MvcRouteTester.Common
{
	internal class Verifier
	{
		private readonly RouteValues expected;
		private readonly RouteValues actual;
		private readonly string url;
		private int expectationsDone = 0;

		public Verifier(RouteValues expected, RouteValues actual, string url)
		{
			this.expected = expected;
			this.actual = actual;
			this.url = url;
		}

		public void VerifyExpectations()
		{
			VerifyValue(expected.Controller, actual.Controller, "controller");
			VerifyValue(expected.Action, actual.Action, "action");
			VerifyValue(expected.Area, actual.Area, "area");

			foreach (var expectedValue in expected.Values)
			{
				var actualValue = actual.GetRouteValue(expectedValue.Name, expectedValue.FromBody);
				if (actualValue == null)
				{
					var notFoundErrorMessage = string.Format("Expected '{0}', got missing value for '{1}' at url '{2}'.",
						expectedValue.Value, expectedValue.Name, url);
					Asserts.Fail(notFoundErrorMessage);
					return;
				}

				VerifyValue(expectedValue.ValueAsString, actualValue.ValueAsString, expectedValue.Name);

				expectationsDone++;
			}

			if (expectationsDone == 0)
			{
				var message = string.Format("No expectations were found for url '{0}'", url);
				Asserts.Fail(message);
			}
		}

		private void VerifyValue(string expectedValue, string actualValue, string name)
		{
			if (string.IsNullOrEmpty(expectedValue))
			{
				return;
			}

			if (string.IsNullOrEmpty(actualValue))
			{
				var notFoundErrorMessage = string.Format("Expected '{0}', got no value for '{1}' at url '{2}'.",
					expectedValue, name, url);
				Asserts.Fail(notFoundErrorMessage);
				return;
			}

			var mismatchErrorMessage = string.Format("Expected '{0}', not '{1}' for '{2}' at url '{3}'.",
				expectedValue, actualValue, name, url);
			Asserts.StringsEqualIgnoringCase(expectedValue, actualValue, mismatchErrorMessage);

			expectationsDone++;
		}
	}
}
