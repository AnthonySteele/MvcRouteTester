using System.Collections.Generic;
using System.Web.Routing;

using MvcRouteTester.Assertions;

namespace MvcRouteTester.WebRoute
{
	public class RouteProperties
	{
		public string Controller { get; private set; }
		public string Action { get; private set; }
		public bool DataOk { get; private set; }

		public RouteValueDictionary RouteValues { get; private set; }

		public RouteProperties(IDictionary<string, string> fromProps)
		{
			RouteValues = new RouteValueDictionary();
			CheckRequiredKeysPresent(fromProps);
			if (DataOk)
			{
				ReadProperties(fromProps);
			}
		}

		private void CheckRequiredKeysPresent(IDictionary<string, string> fromProps)
		{
			DataOk = true;

			if (!fromProps.ContainsKey("controller"))
			{
				var message = string.Format("No 'controller' property found in fromProps");
				Asserts.Fail(message);
				DataOk = false;
			}

			if (!fromProps.ContainsKey("action"))
			{
				var message = string.Format("No 'action' property found in fromProps");
				Asserts.Fail(message);
				DataOk = false;
			}
		}

		private void ReadProperties(IDictionary<string, string> fromProps)
		{
			foreach (var fromProp in fromProps)
			{
				switch (fromProp.Key)
				{
					case "controller":
						Controller = fromProp.Value;
						break;

					case "action":
						Action = fromProp.Value;
						break;

					default:
						RouteValues.Add(fromProp.Key, fromProp.Value);
						break;
				}
			}
		}
	}
}
