using System;
using System.Linq.Expressions;
using System.Web.Http;

using MvcRouteTester.Fluent;

using NUnit.Framework;

namespace MvcRouteTester.Test.Fluent
{
	public class WithFromBodyParams
	{
		public static int Method(int a, [FromBody]string b)
		{
			return 0;
		}
	}

	[TestFixture]
	public class AttributeRecogniserTests
	{
		[Test]
		public void ParametersCanBeCheckedForFromBody()
		{
			Expression<Func<int>> expr = () => WithFromBodyParams.Method(1, "");
			var methodCall = (MethodCallExpression)expr.Body;
			var parameters = methodCall.Method.GetParameters();

			AttributeRecogniser recogniser = new AttributeRecogniser();

			Assert.IsFalse(recogniser.IsFromBody(parameters[0]));
			Assert.IsTrue(recogniser.IsFromBody(parameters[1]));
		}
	}
}
