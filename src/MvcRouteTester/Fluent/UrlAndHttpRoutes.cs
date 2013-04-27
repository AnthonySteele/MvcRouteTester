using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;
using MvcRouteTester.ApiRoute;

namespace MvcRouteTester.Fluent
{
	public class UrlAndHttpRoutes
	{
		private string requestBody = string.Empty;

		public UrlAndHttpRoutes(HttpConfiguration configuration, string url)
		{
			Configuration = configuration;
			Url = url;
		}

		public string Url { get; private set; }
		public HttpConfiguration Configuration { get; private set; }

		public UrlAndHttpRoutes WithBody(string body)
		{
			requestBody = body;
			return this;
		}

		public void To<TController>(HttpMethod httpMethod, Expression<Func<TController, object>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			IDictionary<string, string> expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, httpMethod, requestBody, expectedProps);
		}

		public void ToNoRoute()
		{
			ApiRouteAssert.NoRoute(Configuration, Url);
		}

		public void ToNoMethod<TController>(HttpMethod httpMethod) where TController : ApiController
		{
			var controllerType = typeof(TController);
			ApiRouteAssert.RouteDoesNotHaveMethod(Configuration, Url, controllerType, httpMethod);
		}
	}
}