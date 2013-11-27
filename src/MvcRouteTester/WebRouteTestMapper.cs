using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRouteTester
{
	public static class WebRouteTestMapper
	{
		public static void MapAttributeRoutesInAssembly(this RouteCollection routes, Type type)
		{
			MapAttributeRoutesInAssembly(routes, type.Assembly);
		}

		public static void MapAttributeRoutesInAssembly(this RouteCollection routes, Assembly assembly)
		{
			var controllers = (assembly.GetExportedTypes()
				.Where(IsControllerType)).ToList();

			MapAttributeRoutesInControllers(routes, controllers);
		}

		private static bool IsControllerType(Type t)
		{
			return t != null && t.IsPublic && !t.IsAbstract  && 
				t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) 
				&& typeof(IController).IsAssignableFrom(t);
		}

		public static void MapAttributeRoutesInControllers(this RouteCollection routes, IEnumerable<Type> controllers)
		{
			var mapMvcAttributeRoutesMethod = typeof(RouteCollectionAttributeRoutingExtensions)
				.GetMethod("MapMvcAttributeRoutes", 
				BindingFlags.NonPublic | BindingFlags.Static,
				null,
				new[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
				null);

			if (mapMvcAttributeRoutesMethod == null)
			{
				const string MissingMethodMessage = "Internal method System.Web.Mvc.RouteCollectionAttributeRoutingExtensions.MapMvcAttributeRoutes not found. " +
					"You may have updated ASP MVC to a version later than 5.0. " +
					" Check online fir a new version of MvcRouteTester";
				throw new ApplicationException(MissingMethodMessage);
			}

			mapMvcAttributeRoutesMethod.Invoke(null, new object[] { routes, controllers });
		}
	}
}
