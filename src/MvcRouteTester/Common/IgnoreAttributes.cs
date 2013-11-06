using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcRouteTester.Common
{
	internal class IgnoreAttributes
	{
		private readonly HashSet<Type> attributesToIgnore = new HashSet<Type>();

		internal void Add(IEnumerable<Type> types)
		{
			foreach (var type in types)
			{
				Add(type);
			}
		}

		internal void Add(Type type)
		{
			attributesToIgnore.Add(type);
		}

		internal void Clear()
		{
			attributesToIgnore.Clear();
		}

		public bool PropertyIsIgnored(PropertyInfo prop)
		{
			return attributesToIgnore.Any(attr => prop.GetCustomAttributes(attr, true).Length > 0);
		}
	}
}
