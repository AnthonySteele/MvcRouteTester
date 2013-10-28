using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Web.Http;
using MvcRouteTester.ApiRoute;

namespace MvcRouteTester.Fluent
{
	public class UrlAndHttpRoutes
	{
		private string requestBody = string.Empty;
		private BodyFormat bodyFormat = BodyFormat.None;

		public UrlAndHttpRoutes(HttpConfiguration configuration, string url)
		{
			Configuration = configuration;
			Url = url;
		}

		public string Url { get; private set; }
		public HttpConfiguration Configuration { get; private set; }

		[Obsolete("Renamed to WithFormUrlBody")]
		public UrlAndHttpRoutes WithBody(string body)
		{
			return WithFormUrlBody(body);
		}

		public UrlAndHttpRoutes WithFormUrlBody(string body)
		{
			requestBody = body;
			bodyFormat = BodyFormat.FormUrl;
			return this;
		}

		public UrlAndHttpRoutes WithJsonBody(string body)
		{
			requestBody = body;
			bodyFormat = BodyFormat.Json;
			return this;
		}
		
		public void To<TController>(HttpMethod httpMethod, Expression<Func<TController, object>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			var expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, httpMethod, requestBody, bodyFormat, expectedProps);
		}

		public void To<TController>(HttpMethod httpMethod, Expression<Action<TController>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			var expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, httpMethod, requestBody, bodyFormat, expectedProps);
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