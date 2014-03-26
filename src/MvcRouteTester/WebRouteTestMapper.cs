using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

using MvcRouteTester.Common;

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
			var mvcAssemblyReflector = new MvcAssemblyReflector();
			mvcAssemblyReflector.MapAttributeRoutes(routes, controllers);
		}
	}
}
