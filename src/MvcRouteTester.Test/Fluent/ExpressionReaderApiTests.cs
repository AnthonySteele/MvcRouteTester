using System;
using System.Linq.Expressions;
using System.Web.Http;

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
		public void ReadApiReturnsDictionary()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Get();
			var result = reader.Read(args);

			Assert.That(result, Is.Not.Null);
			Assert.That(result.Count, Is.GreaterThan(0));
		}

		[Test]
		public void ReadGetsApiControllerAndAction()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Get();
			var result = reader.Read(args);

			Assert.That(result["controller"], Is.EqualTo("TestApi"));
			Assert.That(result["action"], Is.EqualTo("Get"));
		}

		[Test]
		public void ReadGetsMethodParam()
		{
			var reader = new ExpressionReader();

			Expression<Func<TestApiController, object>> args = c => c.Post(42);
			var result = reader.Read(args);

			Assert.That(result["controller"], Is.EqualTo("TestApi"));
			Assert.That(result["action"], Is.EqualTo("Post"));
			Assert.That(result["id"], Is.EqualTo("42"));
		}

		[Test]
		public void CanReadVoidMethod()
		{
			var reader = new ExpressionReader();

			Expression<Action<MemberController>> args = c => c.DoNothing();
			var result = reader.Read(args);

			Assert.That(result["controller"], Is.EqualTo("Member"));
			Assert.That(result["action"], Is.EqualTo("DoNothing"));
		}
	}
}
