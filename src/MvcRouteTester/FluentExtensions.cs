using System.Web.Routing;

namespace MvcRouteTester
{
	/// <summary>
	/// route.ShouldMap("/foo").To<HomeController>(x => x.Index())
	/// </summary>
	public static class FluentExtensions
	{
		public static FluentToObject ShouldMap(this RouteCollection routes, string url)
		{
			return new FluentToObject(routes, url);
		}
	}
}
