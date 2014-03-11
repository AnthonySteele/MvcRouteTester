using System.Net.Http;
using System.Web.Routing;

using MvcRouteTester.Fluent;

namespace MvcRouteTester
{
	public static class FluentExtensions
	{
		public static UrlAndRoutes ShouldMap(this RouteCollection routes, string url)
		{
			return ShouldMap(routes, HttpMethod.Get, url);
		}

		public static UrlAndRoutes ShouldMap(this RouteCollection routes, HttpMethod httpMethod, string url)
		{
			return new UrlAndRoutes(routes, httpMethod, url);
		}
	}
}
