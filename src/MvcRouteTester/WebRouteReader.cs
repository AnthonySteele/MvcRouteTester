using System.Collections.Generic;
using System.Web.Routing;

namespace MvcRouteTester
{
	internal class WebRouteReader
	{
		public RouteData GetRouteDataForUrl(RouteCollection routes, string url)
		{
			var httpContext = Mockery.ContextForUrl(url);
			return routes.GetRouteData(httpContext);
		}

		public IDictionary<string, string> GetRouteProperties(RouteData routeData)
		{
			var propertyList = new Dictionary<string, string>();

			foreach (var routeValue in routeData.Values)
			{
				propertyList.Add(routeValue.Key, routeValue.Value.ToString());
			}

			return propertyList;
		}
	}
}
