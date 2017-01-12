// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Routing;

namespace MvcRouteTester.WebRoute
{
	public static class DirectRouteExtensions
	{
		public static bool HasDirectRouteMatch(this RouteData routeData)
		{
			if (routeData == null)
			{
				//throw Error.ArgumentNull("routeData");
				throw new NotImplementedException();
			}

			return routeData.Values.ContainsKey(RouteDataTokenKeys.DirectRouteMatches);
		}

		public static IEnumerable<RouteData> GetDirectRouteMatches(this RouteData routeData)
		{
			return GetRouteDataValue<IEnumerable<RouteData>>(routeData, RouteDataTokenKeys.DirectRouteMatches) ?? Enumerable.Empty<RouteData>();
		}

		private static T GetRouteDataValue<T>(this RouteData routeData, string key)
		{
			if (routeData == null)
			{
				//throw Error.ArgumentNull("routeData");
				throw new NotImplementedException();
			}

			if (key == null)
			{
				//throw Error.ArgumentNull("key");
				throw new NotImplementedException();
			}

			object value;
			if (routeData.Values.TryGetValue(key, out value))
			{
				return (T)value;
			}
			else
			{
				return default(T);
			}
		}
	}

	public class RouteDataTokenKeys
	{
		public const string DirectRouteMatches = "MS_DirectRouteMatches";
	}
}