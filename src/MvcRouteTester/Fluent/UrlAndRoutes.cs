using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MvcRouteTester.WebRoute;

namespace MvcRouteTester.Fluent
{
	public class UrlAndRoutes
	{
		private string requestBody = string.Empty;

		public UrlAndRoutes(RouteCollection routes, string url)
		{
			Routes = routes;
			Url = url;
		}

		public string Url { get; private set; }
		public RouteCollection Routes { get; private set; }

		public UrlAndRoutes WithBody(string body)
		{
			requestBody = body;
			return this;
		}

		public void To<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
		{
			var expressionReader = new ExpressionReader();
			IDictionary<string, string> expectedProps = expressionReader.Read(action);

			WebRouteAssert.HasRoute(Routes, HttpMethod.Get, Url, requestBody, expectedProps);
		}

		public void ToNoRoute()
		{
			WebRouteAssert.NoRoute(Routes, Url);
		}

		public void ToIgnoredRoute()
		{
			WebRouteAssert.IsIgnoredRoute(Routes, Url);
		}

		public void ToNonIgnoredRoute()
		{
			WebRouteAssert.IsNotIgnoredRoute(Routes, Url);
		}
	}
}