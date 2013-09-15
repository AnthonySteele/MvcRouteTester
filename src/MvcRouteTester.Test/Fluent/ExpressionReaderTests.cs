using System;
using System.Linq.Expressions;
using System.Web.Mvc;

using MvcRouteTester.Common;
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
		public void ReadReturnsRouteValues()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Controller, Is.EqualTo("Test"));
			Assert.That(result.Action, Is.EqualTo("Index"));
			Assert.That(result.Area, Is.Empty);

			Assert.That(result.Values, Is.Not.Null);
			Assert.That(result.Values.Count, Is.EqualTo(0));
		}

		[Test]
		public void ReadReturnsValidRouteValues()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);
			result.CheckDataOk();

			Assert.That(result.DataOk, Is.True);
		}

		[Test]
		public void ReadGetsControllerAndAction()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);

			Assert.That(result.Controller, Is.EqualTo("Test"));
			Assert.That(result.Action, Is.EqualTo("Index"));
		}

		[Test]
		public void ReadGetsMethodParams()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.GetItem(42);
			var result = reader.Read(args);

			Assert.That(result.Controller, Is.EqualTo("Test"));
			Assert.That(result.Action, Is.EqualTo("GetItem"));
			Assert.That(result.Values.ValueByName("id"), Is.EqualTo(42));
		}

		[Test]
		public void ReadGetsAreaNameWhenPresent()
		{
			var reader = new ExpressionReader();

			Expression<Func<Areas.SomeArea.Controllers.TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);

			Assert.That(result.Controller, Is.EqualTo("Test"));
			Assert.That(result.Action, Is.EqualTo("Index"));
			Assert.That(result.Area, Is.EqualTo("SomeArea"));
		}

		[Test]
		public void ReadDoesNotGetAreaNameWHenNotPresent()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestController, ActionResult>> args = c => c.Index();
			var result = reader.Read(args);

			Assert.That(result.Area, Is.Empty);
		}
	}
}
