using System.Collections.Generic;
using MvcRouteTester.Assertions;

namespace MvcRouteTester.Common
{
	internal class Verifier
	{
		public void VerifyExpectations(IDictionary<string, string> expectations, IDictionary<string, string> routeProperties, string url)
		{
			int expectationsDone = 0;
			foreach (var propertyKey in expectations.Keys)
			{
				var expectedValue = expectations[propertyKey];

				if (!routeProperties.ContainsKey(propertyKey))
				{
					var notFoundErrorMessage = string.Format("Expected '{0}', got no value for '{1}' at url '{2}'.",
						expectedValue, propertyKey, url);
					Asserts.Fail(notFoundErrorMessage);
					return;
				}

				var actualValue = routeProperties[propertyKey];

				var mismatchErrorMessage = string.Format("Expected '{0}', not '{1}' for '{2}' at url '{3}'.",
					expectedValue, actualValue, propertyKey, url);

				Asserts.StringsEqualIgnoringCase(expectedValue, actualValue, mismatchErrorMessage);

				expectationsDone++;
			}

			if (expectationsDone == 0)
			{
				var message = string.Format("No expectations were found for url '{0}'", url);
				Asserts.Fail(message);
			}
		}
	}
}
