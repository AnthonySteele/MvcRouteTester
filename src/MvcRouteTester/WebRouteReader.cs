using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Routing;

namespace MvcRouteTester
{
	internal class WebRouteReader
	{
		public IDictionary<string, string> GetRouteProperties(RouteData routeData, NameValueCollection requestParams)
		{
			var propertyList = new Dictionary<string, string>();

			foreach (var routeValue in routeData.Values)
			{
				propertyList.Add(routeValue.Key, routeValue.Value.ToString());
			}

			foreach (var paramName in requestParams.AllKeys)
			{
				if (propertyList.ContainsKey(paramName))
				{
					propertyList[paramName] = requestParams[paramName];
				}
				else
				{
					propertyList.Add(paramName, requestParams[paramName]);
				}
			}

			return propertyList;
		}
	}
}
