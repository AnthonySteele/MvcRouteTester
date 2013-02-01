using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRouteTester.Fluent
{
	public class UrlAndRoute
	{
		public UrlAndRoute(RouteCollection routes, string url)
		{
			Routes = routes;
			Url = url;
		}

		public string Url { get; private set; }
		public RouteCollection Routes { get; private set; }

		public void To<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
		{
			var expressionReader = new ExpressionReader();
			IDictionary<string, string> expectedProps = expressionReader.Read(action);

			RouteAssert.HasRoute(Routes, Url, expectedProps);
		}
	}
}