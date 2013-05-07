using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Routing;

using MvcRouteTester.ApiRoute;

namespace MvcRouteTester.WebRoute
{
	internal class Reader
	{
		public IDictionary<string, string> GetRequestProperties(RouteData routeData, HttpRequestBase request)
		{
			var requestParams = request.Params;
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

			ReadPropertiesFromBodyContent(request, propertyList);

			return propertyList;
		}

		private void ReadPropertiesFromBodyContent(HttpRequestBase request, Dictionary<string, string> actualProps)
		{
			var body = GetRequestBody(request);
			if (!string.IsNullOrEmpty(body))
			{
				var bodyReader = new BodyReader();
				bodyReader.ReadBody(body, actualProps);
			}
		}

		private string GetRequestBody(HttpRequestBase request)
		{
			using (Stream receiveStream = request.InputStream)
			{
				using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
				{
					return readStream.ReadToEnd();
				}
			}
		}
	}
}
