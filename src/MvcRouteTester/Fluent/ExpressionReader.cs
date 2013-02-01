using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcRouteTester.Fluent
{
	public class ExpressionReader
	{
		public IDictionary<string, string> Read<TController>(Expression<Func<TController, ActionResult>> action) where TController : Controller
		{
			var methodCall = (MethodCallExpression)action.Body;

			var values = new Dictionary<string, string>();

			values.Add("controller", ControllerName(typeof(TController)));
			values.Add("action", ActionName(methodCall));
			AddParameters(methodCall, values);

			return values;
		}

		private string ControllerName(Type controllertype)
		{
			const int LengthOfTheWordController = 10;
			var controllerName = controllertype.Name;
			if ((controllerName.Length > LengthOfTheWordController) && controllerName.EndsWith("Controller"))
			{
				controllerName = controllerName.Substring(0, controllerName.Length - LengthOfTheWordController);
			}

			return controllerName;
		}

		private string ActionName(MethodCallExpression methodCall)
		{
			return  methodCall.Method.Name;
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

		private static object GetExpectedValue(Expression argumentExpression)
		{
			switch (argumentExpression.NodeType)
			{
				case ExpressionType.Constant:
					return ((ConstantExpression)argumentExpression).Value;

				case ExpressionType.MemberAccess:
					return Expression.Lambda(argumentExpression).Compile().DynamicInvoke();
				
				default:
					return null;
			}
		} 
	}
}