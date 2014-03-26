using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRouteTester.Common
{
    public static class DirectRouteHelper
    {
        public static RouteData FilterRouteData(RouteData routeData, HttpContextBase httpContext)
        {
            // THis method copies logic from System.Web.Mvc.ControllerActionInvoker.FindAction

            if (HasDirectRouteMatchFor(routeData))
            {
                var controllerContext = GetControllerContext(httpContext, routeData);

                var candidates = GetDirectRouteCandidatesFor(controllerContext);

                var bestCandidate = SelectBestCandidate(candidates, controllerContext);
                if (bestCandidate == null)
                {
                    return null;
                }
                else
                {
                    return GetRouteDataFrom(bestCandidate);
                }
            }
            else
            {
                return routeData;
            }
        }

        private static ControllerContext GetControllerContext(HttpContextBase httpContext, RouteData routeData)
        {
            var requestContext = new RequestContext(httpContext, routeData);
            var controllerContext = new ControllerContext();
            controllerContext.RequestContext = requestContext;
            return controllerContext;
        }

        private static object GetDirectRouteCandidatesFor(ControllerContext controllerContext)
        {
            var methodInfo = typeof(ControllerActionInvoker).GetMethod("GetDirectRouteCandidates", BindingFlags.NonPublic | BindingFlags.Static);
            var candidates = methodInfo.Invoke(null, new object[] { controllerContext });
            return candidates;
        }

        private static RouteData GetRouteDataFrom(object bestCandidate)
        {
            var mvcAssembly = Assembly.Load("System.Web.Mvc, Version=5.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            var directRouteCandidateType = mvcAssembly.GetType("System.Web.Mvc.Routing.DirectRouteCandidate");
            var routeDataProperty = directRouteCandidateType.GetProperty("RouteData");
            var routeData = (RouteData)routeDataProperty.GetValue(bestCandidate);
            return routeData;
        }

        private static bool HasDirectRouteMatchFor(RouteData routeData)
        {
            if (routeData == null) throw new ArgumentNullException("routeData");

            var mvcAssembly = Assembly.Load("System.Web.Mvc, Version=5.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            var directRouteExtensionsType = mvcAssembly.GetType("System.Web.Mvc.Routing.DirectRouteExtensions");
            var hasDirectRouteMatchMethod = directRouteExtensionsType.GetMethod("HasDirectRouteMatch", BindingFlags.Public | BindingFlags.Static);
            var hasDirectRouteMatch = (bool)hasDirectRouteMatchMethod.Invoke(null, new object[] { routeData });
            return hasDirectRouteMatch;
        }

        private static object SelectBestCandidate(object candidates, ControllerContext controllerContext)
        {
            var mvcAssembly = Assembly.Load("System.Web.Mvc, Version=5.1.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
            var directRouteCandidateType = mvcAssembly.GetType("System.Web.Mvc.Routing.DirectRouteCandidate");
            var selectBestCandidateMethod = directRouteCandidateType.GetMethod("SelectBestCandidate", BindingFlags.Public | BindingFlags.Static);
            var bestCandidate = selectBestCandidateMethod.Invoke(null, new object[] { candidates, controllerContext });
            return bestCandidate;
        }
    }
}