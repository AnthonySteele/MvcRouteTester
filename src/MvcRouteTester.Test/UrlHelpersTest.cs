using NUnit.Framework;

namespace MvcRouteTester.Test
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
		public void EmptyRelativeUrlIsPrefixed()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("~/");

			Assert.That(outputUrl, Is.StringStarting("http://"));
		}

		[Test]
		public void NonEmptyRelativeUrlIsPrefixed()
		{
			var outputUrl = UrlHelpers.MakeAbsolute("~/customers/1");

			Assert.That(outputUrl, Is.StringStarting("http://"));
		}
	}
}
