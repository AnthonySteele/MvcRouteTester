using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Fluent
{
	public class UrlAndHttpRoutes
	{
		public UrlAndHttpRoutes(HttpConfiguration configuration, string url)
		{
			Configuration = configuration;
			Url = url;
		}

		public string Url { get; private set; }
		public HttpConfiguration Configuration { get; private set; }

		public void To<TController>(HttpMethod httpMethod, Expression<Func<TController, object>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			IDictionary<string, string> expectedProps = expressionReader.Read(action);

			RouteAssert.HasApiRoute(Configuration, Url, httpMethod, expectedProps);
		}

		public void ToNoRoute()
		{
			RouteAssert.NoApiRoute(Configuration, Url);
		}

		public void ToNoMethod<TController>(HttpMethod httpMethod) where TController : ApiController
		{
			var controllerType = typeof(TController);
			RouteAssert.ApiRouteDoesNotHaveMethod(Configuration, Url, controllerType, httpMethod);
		}
	}
}