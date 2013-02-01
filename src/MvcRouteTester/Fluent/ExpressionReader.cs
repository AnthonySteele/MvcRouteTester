using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace MvcRouteTester.Fluent
{
	public class ExpressionReader
	{
		public IDictionary<string, string> Read<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
		{
			var methodCall = (MethodCallExpression)action.Body;

			var values = new Dictionary<string, string>();

			AddControllerName<TController>(values);
			AddActionName(methodCall, values);
			AddParameters(methodCall, values);

			return values;
		}

		private void AddControllerName<T>(Dictionary<string, string> values)
		{
			var controllerName = typeof(T).Name;
			if (controllerName.EndsWith("Controller"))
			{
				controllerName = controllerName.Substring(0, controllerName.Length - 10);
			}

			values.Add("Controller", controllerName);
		}

		private void AddActionName(MethodCallExpression methodCall, Dictionary<string, string> values)
		{
			var actionName = methodCall.Method.Name;
			values.Add("Action", actionName);
		}

		private void AddParameters(MethodCallExpression methodCall, IDictionary<string, string> values)
		{
			var parameters = methodCall.Method.GetParameters();
			var arguments = methodCall.Arguments;

			for (int i = 0; i < parameters.Length; i++)
			{
				var expectedValue = GetExpectedValue(arguments[i]);
				var expectedString = expectedValue != null ? expectedValue.ToString() : null;

				values.Add(parameters[i].Name, expectedString);
			}
		}

		private static object GetExpectedValue(Expression theArgument)
		{
			switch (theArgument.NodeType)
			{
				case ExpressionType.Constant:
					return ((ConstantExpression)theArgument).Value;

				case ExpressionType.MemberAccess:
					return Expression.Lambda(theArgument).Compile().DynamicInvoke();
				
				default:
					return null;
			}
		} 
	}
}