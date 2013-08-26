using System;
using System.Collections.Generic;
using System.Reflection;

namespace MvcRouteTester.Common
{
	public class PropertyReader
	{
		public  bool IsSimpleType(Type type)
		{
			if (type.Name == "Nullable`1")
			{
				return true;
			}

			return type.IsPrimitive || 
				(type == typeof(string)) || (type == typeof(decimal)) || (type == typeof(Guid));
		}

		public IDictionary<string, string> Properties(object dataObject)
		{
			if (dataObject == null)
			{
				return new Dictionary<string, string>();
			}

			var type = dataObject.GetType();
			var objectProperties = GetPublicObjectProperties(type);

			var result = new Dictionary<string, string>();
			foreach (PropertyInfo objectProperty in objectProperties)
			{
				if (IsSimpleType(objectProperty.PropertyType))
				{
					var value = GetPropertyValue(dataObject, objectProperty);
					result.Add(objectProperty.Name, ValueAsString(value));
				}
			}

			return result;
		}

		private object GetPropertyValue(object dataObject, PropertyInfo objectProperty)
		{
			var getMethod = objectProperty.GetGetMethod();
			return getMethod.Invoke(dataObject, null);
		}

		public IEnumerable<string> SimplePropertyNames(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			var objectProperties = GetPublicObjectProperties(type);

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

		private static IEnumerable<PropertyInfo> GetPublicObjectProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		}

		private static string ValueAsString(object propertyValue)
		{
			return propertyValue == null ? null : propertyValue.ToString();
		}
	}
}
