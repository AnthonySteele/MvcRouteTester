using System.Web.Routing;

using MvcRouteTester.Fluent;

namespace MvcRouteTester
{
	public static class FluentExtensions
	{
		public static UrlAndRoute ShouldMap(this RouteCollection routes, string url)
		{
			return new UrlAndRoute(routes, url);
		}
	}
}
