using System.Web.Http;

using MvcRouteTester.Fluent;

namespace MvcRouteTester
{
	public static class FluentApiExtensions
	{
		public static UrlAndHttpRoutes ShouldMap(this HttpConfiguration configuration, string url)
		{
			return new UrlAndHttpRoutes(configuration, url);
		}
	}
}
