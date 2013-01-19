using NUnit.Framework;

namespace MvcRouteTester.Test
{
	[TestFixture]
	public class MockeryTest
	{
		[Test]
		public void ShouldReturnContextForUrl()
		{
			var context = Mockery.ContextForUrl("/foo/bar");

			Assert.That(context.Request.AppRelativeCurrentExecutionFilePath, Is.EqualTo("/foo/bar"));
		}
	}
}
