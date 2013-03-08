using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace MvcRouteTester.Fluent
{
	public class ExpressionReader
	{
		public IDictionary<string, string> Read<TController>(Expression<Func<TController, object>> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			return Read(typeof(TController), (MethodCallExpression)action.Body);
		}

		public IDictionary<string, string> Read<TController>(Expression<Func<TController, ActionResult>> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			return Read(typeof(TController), (MethodCallExpression)action.Body);
		}

		private IDictionary<string, string> Read(Type controllerType, MethodCallExpression methodCall)
		{
			var values = new Dictionary<string, string>();
			values.Add("controller", ControllerName(controllerType));
			values.Add("action", ActionName(methodCall));
			AddParameters(methodCall, values);
			return values;
		}

		private string ControllerName(Type controllertype)
		{
			const int SuffixLength = 10;
			var controllerName = controllertype.Name;
			if ((controllerName.Length > SuffixLength) && controllerName.EndsWith("Controller"))
			{
				controllerName = controllerName.Substring(0, controllerName.Length - SuffixLength);
			}

			return controllerName;
		}

		private string ActionName(MethodCallExpression methodCall)
		{
			return  methodCall.Method.Name;
		}

		private void AddParameters(MethodCallExpression methodCall, IDictionary<string, string> values)
		{
			var attributeRecogniser = new AttributeRecogniser();
			var propertyReader = new PropertyReader();

			var parameters = methodCall.Method.GetParameters();
			var arguments = methodCall.Arguments;

			for (int i = 0; i < parameters.Length; i++)
			{
				var param = parameters[i];
				if (!attributeRecogniser.IsFromBody(param))
				{
					var expectedValue = GetExpectedValue(arguments[i]);

					if (propertyReader.IsSimpleType(param.ParameterType))
					{
						var expectedString = expectedValue != null ? expectedValue.ToString() : null;

						values.Add(param.Name, expectedString);
					}
					else
					{
						var objectFieldValues = propertyReader.Properties(expectedValue);
						foreach (var field in objectFieldValues)
						{
							values.Add(field.Key.ToLowerInvariant(), field.Value);
						}
					}

				}
			}
		}

		private static object GetExpectedValue(Expression argumentExpression)
		{
			switch (argumentExpression.NodeType)
			{
				case ExpressionType.Constant:
					return ((ConstantExpression)argumentExpression).Value;

				case ExpressionType.MemberAccess:
				case ExpressionType.Convert:
				case ExpressionType.MemberInit:
					return Expression.Lambda(argumentExpression).Compile().DynamicInvoke();
				
				default:
					return null;
			}
		} 
	}
}