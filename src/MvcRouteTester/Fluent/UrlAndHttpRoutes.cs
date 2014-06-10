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
		private BodyFormat bodyFormat = BodyFormat.None;

		public UrlAndHttpRoutes(HttpConfiguration configuration, HttpMethod httpMethod, string url)
		{
			Configuration = configuration;
			HttpMethod = httpMethod;
			Url = url;
		}

		public HttpConfiguration Configuration { get; private set; }

		public HttpMethod HttpMethod { get; private set; }
		public string Url { get; private set; }

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

	    public UrlAndHttpRoutes To<TController>(Expression<Func<TController, object>> action) where TController : ApiController
	    {
	        return To<TController>((Dictionary<string,string>)null, action);
	    }
        
		public UrlAndHttpRoutes To<TController>(Dictionary<string, string> headers, Expression<Func<TController, object>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			var expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, HttpMethod, headers, requestBody, bodyFormat, expectedProps);

			return this;
		}

	    public UrlAndHttpRoutes To<TController>(HttpMethod httpMethod, Expression<Func<TController, object>> action)
	        where TController : ApiController
	    {
	        return To<TController>(httpMethod, null, action);
	    }

		public UrlAndHttpRoutes To<TController>(HttpMethod httpMethod, Dictionary<string, string> headers, Expression<Func<TController, object>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			var expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, httpMethod, headers, requestBody, bodyFormat, expectedProps);

			return this;
		}

	    public UrlAndHttpRoutes To<TController>(HttpMethod httpMethod, Expression<Action<TController>> action)
	        where TController : ApiController
	    {
	        return To<TController>(httpMethod, null, action);

	    }

		public UrlAndHttpRoutes To<TController>(HttpMethod httpMethod, Dictionary<string, string> headers, Expression<Action<TController>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			var expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, httpMethod, headers, requestBody, bodyFormat, expectedProps);

			return this;
		}

	    public UrlAndHttpRoutes To<TController>(Expression<Action<TController>> action)
	        where TController : ApiController
	    {
	        return To<TController>((Dictionary<string,string>)null, action);

	    }
		public UrlAndHttpRoutes To<TController>(Dictionary<string, string> headers, Expression<Action<TController>> action) where TController : ApiController
		{
			var expressionReader = new ExpressionReader();
			var expectedProps = expressionReader.Read(action);

			ApiRouteAssert.HasRoute(Configuration, Url, HttpMethod, headers, requestBody, bodyFormat, expectedProps);

			return this;
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

		public void WithHandler<THandler>() where THandler : DelegatingHandler
		{
			ApiRouteAssert.HasHandler<THandler>(Configuration,Url);
		}

		public void WithoutHandler<THandler>() where THandler : DelegatingHandler
		{
			ApiRouteAssert.HasNoHandlerofType<THandler>(Configuration, Url);
		}

		public void WithoutHandler()
		{
			ApiRouteAssert.HasNoHandler(Configuration, Url);
		}
	}
}