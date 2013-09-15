using System;
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
			DataOk = true;
		}

		public RouteValues(IDictionary<string, object> values)
		{
			DataOk = true;

			foreach (var value in values)
			{
				AddRouteValue(value.Key, value.Value);
			}
		}

		public RouteValues(IDictionary<string, string> values)
		{
			DataOk = true;

			foreach (var value in values)
			{
				AddRouteValue(value.Key, value.Value);
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
					Add(new RouteValue(key, value, RouteValueOrigin.Unknown));
					break;
			}
		}

		private void AddRouteValue(RouteValue value)
		{
			switch (value.Name.ToLowerInvariant())
			{
				case "controller":
					Controller = value.ValueAsString;
					break;

				case "action":
					Action = value.ValueAsString;
					break;

				case "area":
					Area = value.ValueAsString;
					break;

				default:
					Add(value);
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

		public void AddRangeWithParse(IEnumerable<RouteValue> valuesToAdd)
		{
			foreach (var routeValue in valuesToAdd)
			{
				AddRouteValue(routeValue);
			}
		}

		public IList<RouteValue> Values
		{
			get { return values; }
		}

		public RouteValue GetRouteValue(string name, RouteValueOrigin expectedOrigin)
		{
			foreach (var routeValue in values)
			{
				if (string.Equals(name, routeValue.Name, StringComparison.OrdinalIgnoreCase) && RouteValueOriginHelpers.Matches(expectedOrigin, routeValue.Origin))
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

			if (! string.IsNullOrEmpty(Controller))
			{
				result.Add("controller", Controller);
			}

			if (!string.IsNullOrEmpty(Action))
			{
				result.Add("action", Action);
			}

			if (!string.IsNullOrEmpty(Area))
			{
				result.Add("area", Area);
			}

			return result;
		}

		public void CheckDataOk()
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
	}
}
