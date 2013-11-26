using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Routing;

using MvcRouteTester.ApiRoute;
using MvcRouteTester.Common;

namespace MvcRouteTester.WebRoute
{
	internal class Reader
	{
		public RouteValues GetRequestProperties(RouteData routeData, HttpRequestBase request, BodyFormat bodyFormat)
		{
			RouteValues result;

			if (routeData == null)
			{
				result = new RouteValues();
			}
			else
			{
				result = new RouteValues(routeData.Values);
			}

			var requestParams = ReadRequestParams(request.Params);
			result.AddRange(requestParams);

			var bodyContent = ReadPropertiesFromBodyContent(request, bodyFormat);
			result.AddRange(bodyContent);

			result.Area = ReadAreaFromRouteData(routeData);
			return result;
		}

		private IList<RouteValue> ReadRequestParams(NameValueCollection requestParams)
		{
			var result = new List<RouteValue>();
			foreach (var paramName in requestParams.AllKeys)
			{
				result.Add(new RouteValue(paramName, requestParams[paramName], RouteValueOrigin.Params));
			}

			return result;
		}

		private IList<RouteValue> ReadPropertiesFromBodyContent(HttpRequestBase request, BodyFormat bodyFormat)
		{
			var body = GetRequestBody(request);
			var bodyReader = new BodyReader();
			return bodyReader.ReadBody(body, bodyFormat);
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

		private string ReadAreaFromRouteData(RouteData routeData)
		{
			if (routeData == null)
			{
				return string.Empty;
			}

			if (routeData.DataTokens.ContainsKey("area"))
			{
				return routeData.DataTokens["area"].ToString();
			}

			return string.Empty;
		}
	}
}
