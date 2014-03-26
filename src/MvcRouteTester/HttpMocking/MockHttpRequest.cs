using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace MvcRouteTester.HttpMocking
{
	internal class MockHttpRequest : HttpRequestBase
	{
		private readonly HttpMethod method;
		private readonly string relativeUrl;
		private readonly NameValueCollection queryParams;
		private readonly string requestBody;
		private readonly NameValueCollection headers = new NameValueCollection();
		private RequestContext requestContext;

		public MockHttpRequest(HttpMethod method, string relativeUrl, NameValueCollection queryParams, string requestBody)
		{
			this.method = method;
			this.relativeUrl = relativeUrl;
			this.queryParams = queryParams;
			this.requestBody = requestBody ?? string.Empty;
		}

		public override string AppRelativeCurrentExecutionFilePath
		{
			get { return relativeUrl; }
		}

		public override NameValueCollection ServerVariables
		{
			get
			{
				return new NameValueCollection();
			}
		}

		public override string ApplicationPath
		{
			get { return relativeUrl; }
		}

		public override NameValueCollection QueryString 
		{
			get { return queryParams; }
		}

		public override NameValueCollection Params 
		{
			get { return queryParams; }
		}

		public override string PathInfo 
		{
			get { return String.Empty; }
		}

		public override string HttpMethod
		{
			get { return method.ToString().ToUpperInvariant(); }
		}

		public override NameValueCollection Headers
		{
			get { return headers; }
		}

		public override RequestContext RequestContext
		{
			get { return requestContext; }
		}

		public override NameValueCollection Form
		{
			get { return new NameValueCollection(); }
		}

		public override Stream InputStream
		{
			get
			{
				return new MemoryStream(UTF8Encoding.Default.GetBytes(requestBody));
			}
		}

		public void SetContext(RequestContext context)
		{
			requestContext = context;
		}
	}
}