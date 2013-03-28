using System;
using System.Collections.Specialized;
using System.Web;

namespace MvcRouteTester.HttpMocking
{
	internal class MockHttpRequest : HttpRequestBase
	{
		private readonly string relativeUrl;
		private readonly NameValueCollection queryParams;
        private readonly NameValueCollection headers = new NameValueCollection();

		public MockHttpRequest(string relativeUrl, NameValueCollection queryParams)
		{
			this.relativeUrl = relativeUrl;
			this.queryParams = queryParams;
		}

		public override string AppRelativeCurrentExecutionFilePath
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
            get { return "GET"; }
        }

        public override NameValueCollection Headers
        {
            get { return headers; }
        }
	}
}