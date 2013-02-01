using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace MvcRouteTester.Fluent
{
	public class UrlAndHttpRoutes
	{
		public UrlAndHttpRoutes(HttpConfiguration configuration, string url, HttpMethod httpMethod)
		{
			Configuration = configuration;
			Url = url;
			HttpMethod = httpMethod;
		}

		public string Url { get; private set; }
		public HttpConfiguration Configuration { get; private set; }
		public HttpMethod HttpMethod { get; private set; }

		public void To<TController>(Expression<Func<TController, object>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			IDictionary<string, string> expectedProps = expressionReader.Read(action);

			RouteAssert.HasApiRoute(Configuration, Url, HttpMethod, expectedProps);
		}
	}
}