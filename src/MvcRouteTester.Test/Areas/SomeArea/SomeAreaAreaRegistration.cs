using System.Web.Mvc;

namespace MvcRouteTester.Test.Areas.SomeArea
{
	public class SomeAreaAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "SomeArea";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"SomeArea_TestController",
				"SomeArea/{action}/{id}",
				new { action = "Index", controller = "Test", id = UrlParameter.Optional }
			);
			context.MapRoute(
				"SomeArea_default",
				"SomeArea/{controller}/{action}/{id}",
				new { action = "Index", controller = "Test", id = UrlParameter.Optional }
			);
		}
	}
}
