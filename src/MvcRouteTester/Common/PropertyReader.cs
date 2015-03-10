using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcRouteTester.Common
{
	public class PropertyReader
	{
		private static readonly IgnoreAttributes IgnoreAttributes = new IgnoreAttributes();

		public static void AddIgnoreAttributes(IEnumerable<Type> types)
		{
			IgnoreAttributes.Add(types);
		}

		public static void AddIgnoreAttribute(Type type)
		{
			IgnoreAttributes.Add(type);
		}

		public static void ClearIgnoreAttributes()
		{
			IgnoreAttributes.Clear();
		}

		public bool IsSimpleType(Type type)
		{
			return TypeHelper.CanConvertFromString(type);
		}

		public RouteValues RouteValues(object dataObject)
		{
			var propertiesList = PropertiesList(dataObject);

			var expectedProps = new RouteValues();
			expectedProps.AddRangeWithParse(propertiesList);
			return expectedProps;
		}

		public IList<RouteValue> PropertiesList(object dataObject, RouteValueOrigin origin = RouteValueOrigin.Unknown)
		{
			var result = new List<RouteValue>();
			if (dataObject == null)
			{
				return result;
			}

			var type = dataObject.GetType();
			var objectProperties = GetPublicObjectProperties(type);

			foreach (PropertyInfo objectProperty in objectProperties)
			{
				if (IsSimpleType(objectProperty.PropertyType))
				{
					var value = GetPropertyValue(dataObject, objectProperty);
					result.Add(new RouteValue(objectProperty.Name, value, origin));
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
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(p => ! IgnoreAttributes.PropertyIsIgnored(p))
				.ToList();
		}
	}
}
