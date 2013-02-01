using System.Web.Routing;

using MvcRouteTester.Fluent;

namespace MvcRouteTester
{
	public static class FluentExtensions
	{
		public static UrlAndRoutes ShouldMap(this RouteCollection routes, string url)
		{
			return new UrlAndRoutes(routes, url);
		}
	}
}
