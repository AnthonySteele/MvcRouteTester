using System;
using System.Collections.Generic;

namespace MvcRouteTester.Common
{
	public class UrlHelpers
	{
		public static string DummySitePrefix { get; set; } = "http://site.com";

		public static string MakeAbsolute(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return String.Empty;
			}

			if (url.StartsWith("~"))
			{
				return DummySitePrefix + url.Substring(1);
			}

			if (url.StartsWith("/"))
			{
				return DummySitePrefix + url;
			}

			return url;
		}

		public static string PrependTilde(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return String.Empty;
			}

			if (url.StartsWith("/"))
			{
				return "~" + url;
			}

			return url;
		}

		public static IList<RouteValue> ReadQueryParams(string url)
		{
			var queryParameters = new List<RouteValue>();

			if (string.IsNullOrEmpty(url))
			{
				return queryParameters;
			}

			var routeParts = url.Split('?');
			if (routeParts.Length < 2)
			{
				return queryParameters;
			}

			var paramsString = routeParts[1];

			if (!string.IsNullOrWhiteSpace(paramsString))
			{
				var paramsWithValues = paramsString.Split('&');
				foreach (var paramWithValue in paramsWithValues)
				{
					var nameValuePair = paramWithValue.Split('=');
					string value = string.Empty;
					if (nameValuePair.Length > 1)
					{
						value = nameValuePair[1];
					}

					queryParameters.Add(new RouteValue(nameValuePair[0], value, RouteValueOrigin.Params));
				}
			}

			return queryParameters;
		}
	}
}
