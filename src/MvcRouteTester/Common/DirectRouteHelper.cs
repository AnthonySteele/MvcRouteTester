using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRouteTester.Common
{
	public static class DirectRouteHelper
	{

		/// <summary>
		/// This method copies logic from System.Web.Mvc.ControllerActionInvoker.FindAction
		/// </summary>
		public static RouteData FilterRouteData(RouteData routeData, HttpContextBase httpContext)
		{
			var mvcAssemblyReflector = new MvcAssemblyReflector();

			if (mvcAssemblyReflector.HasDirectRouteMatchFor(routeData))
			{
				var controllerContext = GetControllerContext(httpContext, routeData);

				var candidates = mvcAssemblyReflector.GetDirectRouteCandidatesFor(controllerContext);

				var bestCandidate = mvcAssemblyReflector.SelectBestCandidate(candidates, controllerContext);
				if (bestCandidate == null)
				{
					return null;
				}

				return mvcAssemblyReflector.GetRouteDataFrom(bestCandidate);
			}

			return routeData;
		}

		private static ControllerContext GetControllerContext(HttpContextBase httpContext, RouteData routeData)
		{
			var requestContext = new RequestContext(httpContext, routeData);
			var controllerContext = new ControllerContext();
			controllerContext.RequestContext = requestContext;
			return controllerContext;
		}
	}
}