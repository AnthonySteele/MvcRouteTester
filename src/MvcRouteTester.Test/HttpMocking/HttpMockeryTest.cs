using MvcRouteTester.HttpMocking;
using NUnit.Framework;

namespace MvcRouteTester.Test.HttpMocking
{
	[TestFixture]
	public class HttpMockeryTest
	{
		[Test]
		public void ShouldReturnPathForUrl()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar");

			Assert.That(context.Request.AppRelativeCurrentExecutionFilePath, Is.EqualTo("/foo/bar"));
		}

		[Test]
		public void ShouldReturnPathForUrlWithoutParams()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b");

			Assert.That(context.Request.AppRelativeCurrentExecutionFilePath, Is.EqualTo("/foo/bar"));
		}

		[Test]
		public void ShouldReturnQueryParam()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b");

			var queryParams = context.Request.Params;
			Assert.That(queryParams, Is.Not.Null);
			Assert.That(queryParams.Count, Is.EqualTo(1));
			Assert.That(queryParams["a"], Is.EqualTo("b"));
		}

		[Test]
		public void ShouldReturnMultipleQueryParams()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b&cee=123");

			var queryParams = context.Request.Params;
			Assert.That(queryParams, Is.Not.Null);
			Assert.That(queryParams.Count, Is.EqualTo(2));
			Assert.That(queryParams["a"], Is.EqualTo("b"));
			Assert.That(queryParams["cee"], Is.EqualTo("123"));
		}

		[Test]
		public void ShouldReturnQueryString()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b");

			var queryString = context.Request.QueryString;
			Assert.That(queryString, Is.Not.Null);
			Assert.That(queryString.Count, Is.EqualTo(1));
			Assert.That(queryString["a"], Is.EqualTo("b"));
		}

		[Test]
		public void ShouldHandleMissingParamValue()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b&c=");

			var queryString = context.Request.QueryString;
			Assert.That(queryString, Is.Not.Null);
			Assert.That(queryString.Count, Is.EqualTo(2));
			Assert.That(queryString["a"], Is.EqualTo("b"));
			Assert.That(queryString["c"], Is.EqualTo(string.Empty));
		}

		[Test]
		public void ShouldHandleMissingParamAssign()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b&c");

			var queryString = context.Request.QueryString;
			Assert.That(queryString, Is.Not.Null);
			Assert.That(queryString.Count, Is.EqualTo(2));
			Assert.That(queryString["a"], Is.EqualTo("b"));
			Assert.That(queryString["c"], Is.EqualTo(string.Empty));
		}

		[Test]
		public void ShouldHaveEmptyPathInfo()
		{
			var context = HttpMockery.ContextForUrl("/foo/bar?a=b");

			Assert.That(context.Request.PathInfo, Is.EqualTo(string.Empty));
		}

		[Test]
		public void ShouldHaveStubbedContextCorrectly()
		{
			var context = HttpMockery.ContextForUrl("");
			context.RewritePath("");
			context.RewritePath("", "", "");
		}
	}
}
