using System;
using System.Linq.Expressions;
using System.Web.Http;

using MvcRouteTester.Common;
using MvcRouteTester.Fluent;
using MvcRouteTester.Test.ApiControllers;

using NUnit.Framework;

namespace MvcRouteTester.Test.Fluent
{
	public class TestApiController : ApiController
	{
		public object Get()
		{
			return "Hello, Api.";
		}

		public object Post(int id = 12)
		{
			return "";
		}

	}

	[TestFixture]
	public class ExpressionReaderApiTests
	{
		[Test]
		public void ReadNullObjectThrowsException()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = null;
			Assert.Throws<ArgumentNullException>(() => reader.Read(args));
		}

		[Test]
		public void ReadApiReturnsRouteValues()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Get();
			var result = reader.Read(args);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Controller, Is.EqualTo("TestApi"));
			Assert.That(result.Action, Is.EqualTo("Get"));
			Assert.That(result.Area, Is.Empty);

			Assert.That(result.Values, Is.Not.Null);
			Assert.That(result.Values.Count, Is.EqualTo(0));
		}

		[Test]
		public void ReadReturnsValidRouteValues()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Get();
			var result = reader.Read(args);

			result.CheckDataOk();
			Assert.That(result.DataOk, Is.True);
		}

		[Test]
		public void ReadGetsApiControllerAndAction()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Get();
			var result = reader.Read(args);

			Assert.That(result.Controller, Is.EqualTo("TestApi"));
			Assert.That(result.Action, Is.EqualTo("Get"));
		}

		[Test]
		public void ReadGetsMethodParam()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Post(42);
			var result = reader.Read(args);

			Assert.That(result.Controller, Is.EqualTo("TestApi"));
			Assert.That(result.Action, Is.EqualTo("Post"));
			Assert.That(result.Values.FindByName("id").Value, Is.EqualTo(42));
		}

		[Test]
		public void CanReadVoidMethod()
		{
			var reader = new ExpressionReader();

			Expression<Action<MemberController>> args = c => c.DoNothing();
			var result = reader.Read(args);

			Assert.That(result.Controller, Is.EqualTo("Member"));
			Assert.That(result.Action, Is.EqualTo("DoNothing"));
		}
	}
}
