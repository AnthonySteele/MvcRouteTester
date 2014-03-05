using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Filters;
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
			var attributeRoutingMapperType =  GetInternalMapperType();

			if (attributeRoutingMapperType == null)
			{
				const string MissingTypeMessage = "Internal type System.Web.Mvc.Routing.AttributeRoutingMapper not found. " +
					"You may have updated ASP MVC to a version later than 5.1.1 " +
					" Check online for a new version of MvcRouteTester";
				throw new ApplicationException(MissingTypeMessage);
			}

			var mapMvcAttributeRoutesMethod = GetMapRoutesMethod(attributeRoutingMapperType);

			if (mapMvcAttributeRoutesMethod == null)
			{
				const string MissingMethodMessage = "Internal method System.Web.Mvc.AttributeRoutingMapper.MapAttributeRoutes not found. " +
					"You may have updated ASP MVC to a version later than 5.1.1 " +
					" Check online for a new version of MvcRouteTester";
				throw new ApplicationException(MissingMethodMessage);
			}

			mapMvcAttributeRoutesMethod.Invoke(null, new object[] { routes, controllers });
		}

		private static Type GetInternalMapperType()
		{
			var mvcAssembly = Assembly.Load("System.Web.Mvc, Version=5.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
			if (mvcAssembly == null)
			{
				return null;
			}
			return mvcAssembly.GetType("System.Web.Mvc.Routing.AttributeRoutingMapper");
		}

		private static MethodInfo GetMapRoutesMethod(Type attributeRoutingMapperType)
		{
			return attributeRoutingMapperType.GetMethod(
				"MapAttributeRoutes",
				BindingFlags.Public | BindingFlags.Static,
				null,
				new[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
				null);
		}
	}
}
