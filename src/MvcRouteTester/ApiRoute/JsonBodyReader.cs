using System;
using System.Collections;
using System.Collections.Generic;
using MvcRouteTester.Common;
using Newtonsoft.Json.Linq;

namespace MvcRouteTester.ApiRoute
{
	public class JsonBodyReader
	{
		public IList<RouteValue> ReadBody(string body)
		{
			var values = new List<RouteValue>();

			if (string.IsNullOrWhiteSpace(body))
			{
				return values;
			}

			var json = JObject.Parse(body);

			foreach (var jsonProp in json.Properties())
			{
				var routeValue = new RouteValue(jsonProp.Name, ConvertValue(jsonProp.Value), RouteValueOrigin.Body);
				values.Add(routeValue);
			}

			return values;
		}

		private object ConvertValue(JToken value)
		{
			switch (value.Type)
			{
				case JTokenType.Integer:
					return value.ToObject<int>();

				case JTokenType.Float:
					return value.ToObject<double>();

				case JTokenType.Boolean:
					return value.ToObject<bool>();

				case JTokenType.Date:
					return value.ToObject<DateTime>();

				case JTokenType.Guid:
					return value.ToObject<Guid>();

				case JTokenType.TimeSpan:
					return value.ToObject<TimeSpan>();

				default:
					return value.ToString();
			}
		}
	}
}
