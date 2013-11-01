using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcRouteTester.Common
{
	public class PropertyReader
	{
		private static readonly List<Type> SpecialSimpleTypes = new List<Type>
			{
				typeof(string),
				typeof(decimal),
				typeof(DateTime),
				typeof(Guid)
			};

		public  bool IsSimpleType(Type type)
		{
			if (type.Name == "Nullable`1")
			{
				return true;
			}

			return type.IsPrimitive || SpecialSimpleTypes.Contains(type);
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
		    var props= type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).ToList();
		    props.RemoveAll(p => ignoreAttributes.Any(a => p.GetCustomAttributes(a, true).Length > 0));
		    return props;
		}

	    private static Type[] ignoreAttributes = new Type[0];
        public static void IgnoreAttributes(Type[] types)
        {
            ignoreAttributes = types;
        }
    }
}
