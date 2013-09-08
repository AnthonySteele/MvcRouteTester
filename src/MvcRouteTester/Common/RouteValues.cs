using System.Collections.Generic;

namespace MvcRouteTester.Common
{
	public class RouteValues
	{
		private readonly List<RouteValue> values = new List<RouteValue>(); 
	
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Area { get; set; }

		public RouteValues()
		{
		}

		public RouteValues(IDictionary<string, string> values)
		{
			foreach (var value in values)
			{
				switch (value.Key.ToLowerInvariant())
				{
					case "controller":
						Controller = value.Value;
						break;

					case "action":
						Action = value.Value;
						break;

					case "area":
						Area = value.Value;
						break;

					default:
						Add(new RouteValue(value.Key, value.Value, false));
						break;
				}
			}
		}

		public void Add(RouteValue value)
		{
			values.Add(value);
		}

		public void AddRange(IEnumerable<RouteValue> valuesToAdd)
		{
			values.AddRange(valuesToAdd);
		}

		public IList<RouteValue> Values
		{
			get
			{
				return values;
			}
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
