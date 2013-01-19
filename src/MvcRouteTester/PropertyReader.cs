using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MvcRouteTester
{
	public class PropertyReader
	{
		public IDictionary<string, string> Properties(object dataObject)
		{
			if (dataObject == null)
			{
				throw new ArgumentNullException("dataObject");
			}

			var result = new Dictionary<string, string>();
			var objectProperties = TypeDescriptor.GetProperties(dataObject);

			foreach (PropertyDescriptor objectProperty in objectProperties)
			{
				object propertyValue = objectProperty.GetValue(dataObject);
				result.Add(objectProperty.Name, ValueAsString(propertyValue));
			}

			return result;
		}

		private static string ValueAsString(object propertyValue)
		{
			return propertyValue == null ? null : propertyValue.ToString();
		}
	}
}
