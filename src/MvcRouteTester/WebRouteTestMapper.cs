using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;

using MvcRouteTester.Common;

namespace MvcRouteTester
{
	public static class WebRouteTestMapper
	{
	    public static void MapAttributeRoutesInAssembly(this RouteCollection routes, Type type)
	    {
	        MapAttributeRoutesInAssembly(routes, type, new DefaultInlineConstraintResolver());
	    }

	    public static void MapAttributeRoutesInAssembly(this RouteCollection routes, Type type, IInlineConstraintResolver constraintResolver)
		{
			MapAttributeRoutesInAssembly(routes, type.Assembly, constraintResolver);
		}

	    public static void MapAttributeRoutesInAssembly(this RouteCollection routes, Assembly assembly)
	    {
	        MapAttributeRoutesInAssembly(routes, assembly, new DefaultInlineConstraintResolver());
	    }

	    public static void MapAttributeRoutesInAssembly(this RouteCollection routes, Assembly assembly, IInlineConstraintResolver constraintResolver)
		{
			var controllers = (assembly.GetExportedTypes()
				.Where(IsControllerType)).ToList();

			MapAttributeRoutesInControllers(routes, controllers, constraintResolver);
		}

		private static bool IsControllerType(Type t)
		{
			return t != null && t.IsPublic && !t.IsAbstract  && 
				t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) 
				&& typeof(IController).IsAssignableFrom(t);
		}

		public static void MapAttributeRoutesInControllers(this RouteCollection routes, IEnumerable<Type> controllers, IInlineConstraintResolver constraintResolver)
		{
			var mvcAssemblyReflector = new MvcAssemblyReflector();
			mvcAssemblyReflector.MapAttributeRoutes(routes, controllers, constraintResolver);
		}
	}
}
