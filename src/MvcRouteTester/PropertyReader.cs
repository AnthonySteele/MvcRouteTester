using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace MvcRouteTester
{
	public class PropertyReader
	{
		public  bool IsSimpleType(Type type)
		{
			return type.IsPrimitive || (type == typeof(string));
		}

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

		public IEnumerable<string> SimplePropertyNames(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			var objectProperties = type.GetProperties(
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

			var result = new List<string>();
			foreach (PropertyInfo objectProperty in objectProperties)
			{
				if (IsSimpleType(objectProperty.PropertyType))
				{
					result.Add(objectProperty.Name);
				}
			}

			return result;
		}


		private static string ValueAsString(object propertyValue)
		{
			return propertyValue == null ? null : propertyValue.ToString();
		}
	}
}
