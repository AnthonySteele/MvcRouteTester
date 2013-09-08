using System.Collections.Generic;
using System.Web;

using MvcRouteTester.Common;

namespace MvcRouteTester.ApiRoute
{
	/// <summary>
	/// read the post body that is form-urlencoded
	/// </summary>
	public class BodyReader
	{
		public IList<RouteValue> ReadBody(string body)
		{
			var values = new List<RouteValue>();

			var bodyParams = body.Split('&');
			foreach (var bodyParam in bodyParams)
			{
				values.Add(ReadValue(bodyParam));
			}

			return values;
		}

		private static RouteValue ReadValue(string bodyParam)
		{
			var nameAndValue = bodyParam.Split('=');

			var name = nameAndValue[0].ToLowerInvariant();
			string value = string.Empty;
			if (nameAndValue.Length > 1)
			{
				value = nameAndValue[1];
			}

			return new RouteValue(name, HttpUtility.UrlDecode(value), true);
		}
	}
}
