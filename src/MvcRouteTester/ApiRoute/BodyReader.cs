using System;
using System.Collections.Generic;

using MvcRouteTester.Common;

namespace MvcRouteTester.ApiRoute
{
	public class BodyReader
	{
		public IList<RouteValue> ReadBody(string body, BodyFormat bodyFormat)
		{
			switch (bodyFormat)
			{
				case BodyFormat.FormUrl:
					var formUrlReader = new FormUrlBodyReader();
					return formUrlReader.ReadBody(body);

				case BodyFormat.Json:
					var jsonReader = new JsonBodyReader();
					return jsonReader.ReadBody(body);

				case BodyFormat.None:
					return new List<RouteValue>();

				default:
					throw new ApplicationException("Unknown body format: " + bodyFormat);
			}
		}
	}
}
