using System.Collections.Generic;

namespace MvcRouteTester.Common
{
	public class RouteValues
	{
		private readonly List<RouteValue> values = new List<RouteValue>(); 
	
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Area { get; set; }

		public void Add(RouteValue value)
		{
			values.Add(value);
		}

		public RouteValue GetRouteValue(string name, bool fromBody)
		{
			foreach (var routeValue in values)
			{
				if (name == routeValue.Name && fromBody == routeValue.FromBody)
				{
					return routeValue;
				}
			}

			return null;
		}
	}
}
