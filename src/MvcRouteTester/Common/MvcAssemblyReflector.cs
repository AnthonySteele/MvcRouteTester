using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRouteTester.Common
{
	public class MvcAssemblyReflector
	{
		private const string ErrorSuffix = 
			"You may have updated ASP MVC to a version later than 5.2.2.0 " +
			"Check online for a new version of MvcRouteTester";

		private const string MissingAssemblyMessage = "Assembly System.Web.Mvc, Version=5.2.2.0 not found. " + ErrorSuffix;
		const string MissingTypeMessage = "Internal type {0} not found. " + ErrorSuffix;
		const string MissingMethodMessage = "Internal method {0}.{1} not found. " + ErrorSuffix;

		private static Assembly mvcAssembly;

		public MvcAssemblyReflector()
		{
			LoadMvcAssembly();
		}

		public void MapAttributeRoutes(RouteCollection routes, IEnumerable<Type> controllers)
		{
			var mapMvcAttributeRoutesMethod = GetMapRoutesMethod();
			mapMvcAttributeRoutesMethod.Invoke(null, new object[] { routes, controllers });
		}
		
		public object GetDirectRouteCandidatesFor(ControllerContext controllerContext)
		{
			var method = GetDirectRouteCandidatesMethod();
			return method.Invoke(null, new object[] { controllerContext });
		}

		public RouteData GetRouteDataFrom(object bestCandidate)
		{
			var directRouteCandidateType = GetInternalMvcType("System.Web.Mvc.Routing.DirectRouteCandidate");
			var routeDataProperty = directRouteCandidateType.GetProperty("RouteData");
			var routeData = (RouteData)routeDataProperty.GetValue(bestCandidate);
			return routeData;
		}

		public bool HasDirectRouteMatchFor(RouteData routeData)
		{
			if (routeData == null)
			{
				throw new ArgumentNullException("routeData");
			}

			var hasDirectRouteMatchMethod = GetHasDirectRouteMatchMethod();
			return (bool)hasDirectRouteMatchMethod.Invoke(null, new object[] { routeData });
		}

		public object SelectBestCandidate(object candidates, ControllerContext controllerContext)
		{
			var selectBestCandidateMethod = GetSelectBestCandidateMethod();
			return selectBestCandidateMethod.Invoke(null, new[] { candidates, controllerContext });
		}

		private static void LoadMvcAssembly()
		{
			mvcAssembly = Assembly.Load("System.Web.Mvc, Version=5.2.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");

			if (mvcAssembly == null)
			{
				throw new ApplicationException(MissingAssemblyMessage);
			}
		}

		private static Type GetInternalMvcType(string typeName)
		{
			var resultType = mvcAssembly.GetType(typeName);

			if (resultType == null)
			{
				var message = string.Format(MissingTypeMessage, typeName);
				throw new ApplicationException(message);
			}

			return resultType;
		}

		private static void CheckMethod(MethodInfo methodInfo, string className, string methodName)
		{
			if (methodInfo == null)
			{
				var message = string.Format(MissingMethodMessage, className, methodName);
				throw new ApplicationException(message);
			}
		}

		private static MethodInfo GetMapRoutesMethod()
		{
			var attributeRoutingMapperType = GetInternalMvcType("System.Web.Mvc.Routing.AttributeRoutingMapper");

			var resultMethod = attributeRoutingMapperType.GetMethod("MapAttributeRoutes",
				BindingFlags.Public | BindingFlags.Static,
				null,
				new[] { typeof(RouteCollection), typeof(IEnumerable<Type>) },
				null);

			CheckMethod(resultMethod, "System.Web.Mvc.Routing.AttributeRoutingMapper", "MapAttributeRoutes");

			return resultMethod;
		}

		private static MethodInfo GetHasDirectRouteMatchMethod()
		{
			var directRouteExtensionsType = GetInternalMvcType("System.Web.Mvc.Routing.DirectRouteExtensions");
			var hasDirectRouteMatchMethod = directRouteExtensionsType.GetMethod("HasDirectRouteMatch",
				BindingFlags.Public | BindingFlags.Static);

			CheckMethod(hasDirectRouteMatchMethod, "System.Web.Mvc.Routing.DirectRouteExtensions", "HasDirectRouteMatch");

			return hasDirectRouteMatchMethod;
		}

		private static MethodInfo GetSelectBestCandidateMethod()
		{
			var directRouteCandidateType = GetInternalMvcType("System.Web.Mvc.Routing.DirectRouteCandidate");
			var resultMethod = directRouteCandidateType.GetMethod("SelectBestCandidate", BindingFlags.Public | BindingFlags.Static);

			CheckMethod(resultMethod, "System.Web.Mvc.Routing.DirectRouteCandidate", "SelectBestCandidate");

			return resultMethod;
		}

		private static MethodInfo GetDirectRouteCandidatesMethod()
		{
			var type = typeof(ControllerActionInvoker);
			var methodInfo = type.GetMethod("GetDirectRouteCandidates", BindingFlags.NonPublic | BindingFlags.Static);
			CheckMethod(methodInfo, "System.Web.Mvc.ControllerActionInvoker", "GetDirectRouteCandidates");
			return methodInfo;
		}
	}
}
