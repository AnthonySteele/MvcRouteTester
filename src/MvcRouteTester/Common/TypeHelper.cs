using System;
using System.ComponentModel;

namespace MvcRouteTester.Common
{
	public static class TypeHelper
	{
		public static bool CanConvertFromString(Type destinationType)
		{
			return IsBasicType(UnwrapNullableType(destinationType)) ||
				   HasStringConverter(destinationType);
		}

		private static Type UnwrapNullableType(Type destinationType)
		{
			return Nullable.GetUnderlyingType(destinationType) ?? destinationType;
		}

		public static bool IsBasicType(Type type)
		{
			return type.IsPrimitive ||
				   type.IsEnum ||
				   type == typeof(decimal) ||
				   type == typeof(string) ||
				   type == typeof(DateTime) ||
				   type == typeof(Guid) ||
				   type == typeof(DateTimeOffset) ||
				   type == typeof(TimeSpan) ||
				   type == typeof(Uri);
		}

		public static bool HasStringConverter(Type type)
		{
			return TypeDescriptor.GetConverter(type).CanConvertFrom(typeof(string));
		}
	}
}
