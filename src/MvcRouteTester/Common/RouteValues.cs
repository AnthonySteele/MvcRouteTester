using System.Collections.Generic;
using System.Web.Routing;

using MvcRouteTester.Assertions;

namespace MvcRouteTester.Common
{
	public class RouteValues
	{
		private readonly List<RouteValue> values = new List<RouteValue>(); 
	
		public string Controller { get; set; }
		public string Action { get; set; }
		public string Area { get; set; }
		public bool DataOk { get; private set; }

		public RouteValues()
		{
		}

		public RouteValues(IDictionary<string, object> values)
		{
			foreach (var value in values)
			{
				AddRouteValue(value.Key, value.Value);
			}
		}

		public RouteValues(IDictionary<string, string> values)
		{
			foreach (var value in values)
			{
				AddRouteValue(value.Key, value.Value);
			}
		}

		public void CheckRequiredKeysPresent(IDictionary<string, string> fromProps)
		{
			DataOk = true;

			if (string.IsNullOrEmpty(Controller))
			{
				var message = string.Format("No 'controller' property found in fromProps");
				Asserts.Fail(message);
				DataOk = false;
			}

			if (string.IsNullOrEmpty(Action))
			{
				var message = string.Format("No 'action' property found in fromProps");
				Asserts.Fail(message);
				DataOk = false;
			}
		}

		private void AddRouteValue(string key, object value)
		{
			switch (key.ToLowerInvariant())
			{
				case "controller":
					Controller = value.ToString();
					break;

				case "action":
					Action = value.ToString();
					break;

				case "area":
					Area = value.ToString();
					break;

				default:
					Add(new RouteValue(key, value, false));
					break;
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

		public RouteValueDictionary AsRouteValueDictionary()
		{
			RouteValueDictionary result = new RouteValueDictionary();

			foreach (var routeValue in Values)
			{
				result.Add(routeValue.Name, routeValue.Value);
			}

			return result;
		}
	}
}
