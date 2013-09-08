using MvcRouteTester.Common;
using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	[TestFixture]
	public class UrlHelpersTest
	{
		[Test]
		public void AbsoluteUrlIsUnchanged()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("http://foo.com");

			Assert.That(outputUrl, Is.EqualTo("http://foo.com"));
		}

		[Test]
		public void AbsoluteHttpsUrlIsUnchanged()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("https://bar.com");

			Assert.That(outputUrl, Is.EqualTo("https://bar.com"));
		}

		[Test]
		public void FtpHttpsUrlIsUnchanged()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("ftp://bar.com/filez.zip");

			Assert.That(outputUrl, Is.EqualTo("ftp://bar.com/filez.zip"));
		}

		[Test]
		public void EmptyRelativeTildeUrlIsPrefixed()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("~/");

			Assert.That(outputUrl, Is.EqualTo("http://site.com/"));
		}

		[Test]
		public void EmptyRelativeSlashUrlIsPrefixed()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("/");

			Assert.That(outputUrl, Is.EqualTo("http://site.com/"));
		}

		[Test]
		public void NonEmptyTildeRelativeUrlIsPrefixed()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("~/customers/1");

			Assert.That(outputUrl, Is.EqualTo("http://site.com/customers/1"));
		}

		[Test]
		public void NonEmptySlashRelativeUrlIsPrefixed()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("/customers/1");

			Assert.That(outputUrl, Is.EqualTo("http://site.com/customers/1"));
		}

		[Test]
		public void SlashPathHasTildePrended()
		{
			var outputUrl = UrlHelpers.PrependTilde("/");

			Assert.That(outputUrl, Is.EqualTo("~/"));
		}

		[Test]
		public void TildeSlashPathIsUnchanged()
		{
			var outputUrl = UrlHelpers.PrependTilde("~/");

			Assert.That(outputUrl, Is.EqualTo("~/"));
		}

		[Test]
		public void PathWithTildeIsUnchanged()
		{
			var outputUrl = UrlHelpers.PrependTilde("~/customers/1");

			Assert.That(outputUrl, Is.EqualTo("~/customers/1"));
		}

		[Test]
		public void PathHasTildePrepended()
		{
			var outputUrl = UrlHelpers.PrependTilde("/customers/1") ;

			Assert.That(outputUrl, Is.EqualTo("~/customers/1"));
		}

		[Test]
		public void UrlWithNoQueryParamsIsParsed()
		{
			const string Url = "/foo/bar";

			var parsedParams = UrlHelpers.ReadQueryParams(Url);

			Assert.That(parsedParams.Count, Is.EqualTo(0));
		}

		[Test]
		public void UrlWithOneQueryParamsIsParsed()
		{
			const string Url = "/foo/bar?id=3";

			var parsedParams = UrlHelpers.ReadQueryParams(Url);

			Assert.That(parsedParams.Count, Is.EqualTo(1));
			Assert.That(parsedParams["id"], Is.EqualTo("3"));
		}

		[Test]
		public void UrlWithTwoQueryParamsIsParsed()
		{
			const string Url = "/foo/bar?name=fish&fish=trout";

			var parsedParams = UrlHelpers.ReadQueryParams(Url);

			Assert.That(parsedParams.Count, Is.EqualTo(2));
			Assert.That(parsedParams["name"], Is.EqualTo("fish"));
			Assert.That(parsedParams["fish"], Is.EqualTo("trout"));
		}
	}
}
