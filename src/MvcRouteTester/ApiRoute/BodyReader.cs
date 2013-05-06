using System.Collections.Generic;
using System.Web;

namespace MvcRouteTester.ApiRoute
{
	/// <summary>
	/// read the post body that is form-urlencoded
	/// </summary>
	public class BodyReader
	{
		public void ReadBody(string body, Dictionary<string, string> values)
		{
			var bodyParams = body.Split('&');
			foreach (var bodyParam in bodyParams)
			{
				var nameAndValue = bodyParam.Split('=');

				var name = nameAndValue[0].ToLowerInvariant();
				string value = string.Empty;
				if (nameAndValue.Length > 1)
				{
					value = nameAndValue[1];
				}

				values.Add(name, HttpUtility.UrlDecode(value));
			}
		}

	}
}
