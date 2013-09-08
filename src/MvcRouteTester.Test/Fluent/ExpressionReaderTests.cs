using System;
using System.Linq.Expressions;
using System.Web.Mvc;

using MvcRouteTester.Fluent;

using NUnit.Framework;

namespace MvcRouteTester.Test.Fluent
{
	public class TestController : Controller
	{
		public ActionResult Index()
		{
			return new EmptyResult();
		}

		public ActionResult GetItem(int id = 12)
		{
			return new EmptyResult();
		}
	}

	[TestFixture]
	public class ExpressionReaderTests
	{
		[Test]
		public void ReadNullActionResultThrowsException()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = null;
			Assert.Throws<ArgumentNullException>(() => reader.Read(args));
		}

		[Test]
		public void ReadReturnsDictionary()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.GreaterThan(0));
		}

		[Test]
		public void ReadGetsControllerAndAction()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);

			Assert.That(result["controller"], Is.EqualTo("Test"));
			Assert.That(result["action"], Is.EqualTo("Index"));
		}

		[Test]
		public void ReadGetsMethodParams()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.GetItem(42);
			var result = reader.Read(args);

			Assert.That(result["controller"], Is.EqualTo("Test"));
			Assert.That(result["action"], Is.EqualTo("GetItem"));
			Assert.That(result["id"], Is.EqualTo("42"));
		}

        [Test]
        public void ReadGetsAreaName()
        {
            var reader = new ExpressionReader();

            Expression<Func<Areas.SomeArea.TestController, ActionResult>> args = c => c.Index();
            var result = reader.Read(args);

            Assert.That(result["controller"], Is.EqualTo("Test"));
            Assert.That(result["action"], Is.EqualTo("Index"));
            Assert.That(result["area"], Is.EqualTo("SomeArea"));
        }

        [Test]
        public void ReadDoesNotGetAreaName()
        {
            var reader = new ExpressionReader();

            Expression<Func<TestController, ActionResult>> args = c => c.Index();
            var result = reader.Read(args);

            Assert.That(result.ContainsKey("area"), Is.False);
        }
	}
}
