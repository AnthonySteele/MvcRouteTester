using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Fluent;

namespace MvcRouteTester
{
	public static class FluentApiExtensions
	{
		public static UrlAndHttpRoutes ShouldMap(this HttpConfiguration configuration, string url)
		{
			return ShouldMap(configuration, HttpMethod.Get, url);
		}

		public static UrlAndHttpRoutes ShouldMap(this HttpConfiguration configuration, HttpMethod httpMethod, string url)
		{
			return new UrlAndHttpRoutes(configuration, httpMethod, url);
		}
	}
}
