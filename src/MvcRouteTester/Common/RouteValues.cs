using System.Collections.Generic;

namespace MvcRouteTester.Common
{
	public class RouteValues
	{
		private readonly List<RouteValue> values = new List<RouteValue>(); 
		public string Controller { get; set; }
		public string Action { get; set; }

		public void Add(RouteValue value)
		{
			values.Add(value);
		}

		public RouteValue GetRouteValue(string name, bool? fromBody = null)
		{
			foreach (var routeValue in values)
			{
				if (name == routeValue.Name)
				{
					if (! fromBody.HasValue || fromBody.Value == routeValue.FromBody)
					{
						return routeValue;
					}
				}
			}

			return null;
		}
	}
}
