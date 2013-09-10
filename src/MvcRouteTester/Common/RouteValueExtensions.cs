using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MvcRouteTester.Common
{
	public static class RouteValueExtensions
	{
		public static RouteValue FindByName(this IEnumerable<RouteValue> routeValues, string name)
		{
			return routeValues.FirstOrDefault(x => x.Name == name);
		}

		public static object ValueByName(this IEnumerable<RouteValue> routeValues, string name)
		{
			var routeValue = routeValues.FindByName(name);
			return routeValue == null ? null : routeValue.Value;
		}

		public static NameValueCollection AsNameValueCollection(this IEnumerable<RouteValue> queryParamsValues)
		{
			var result = new NameValueCollection();
			foreach (var routeValue in queryParamsValues)
			{
				result.Add(routeValue.Name, routeValue.ValueAsString);
			}
			return result;
		}
	}
}
