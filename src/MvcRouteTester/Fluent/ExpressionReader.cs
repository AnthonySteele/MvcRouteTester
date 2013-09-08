using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MvcRouteTester.Common;

namespace MvcRouteTester.Fluent
{
	public class ExpressionReader
	{
		public IDictionary<string, string> Read<TController>(Expression<Func<TController, object>> action)
		{
			return Read(typeof(TController), UnwrapAction(action));
		}

		public IDictionary<string, string> Read<TController>(Expression<Func<TController, ActionResult>> action)
		{
			return Read(typeof(TController), UnwrapAction(action));
		}

		public IDictionary<string, string> Read<TController>(Expression<Action<TController>> action)
		{
			return Read(typeof(TController), UnwrapAction(action));
		}

		private static MethodCallExpression UnwrapAction<TController>(Expression<Action<TController>> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			return UnwrapExpression(action.Body);
		}


		private static MethodCallExpression UnwrapAction<TController, TResult>(Expression<Func<TController, TResult>> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}

			return UnwrapExpression(action.Body);
		}

		private static MethodCallExpression UnwrapExpression(Expression expression)
		{
			var methodCallExpression = expression as MethodCallExpression;
			if (methodCallExpression != null)
			{
				return methodCallExpression;
			}

			var unaryExpression = expression as UnaryExpression;
			if (unaryExpression != null)
			{
				return UnwrapExpression(unaryExpression.Operand);
			}

			throw new ApplicationException("No way to unwrap a " + expression.GetType().FullName);
		}

		private IDictionary<string, string> Read(Type controllerType, MethodCallExpression methodCall)
		{
			var values = new Dictionary<string, string>();
			values.Add("controller", ControllerName(controllerType));
			values.Add("action", ActionName(methodCall));
			AddParameters(methodCall, values);
		    AddArea(controllerType, values);
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
        private void AddArea(Type controllerType, Dictionary<string, string> values)
        {
            var area = AreaName(controllerType);
            if (!String.IsNullOrEmpty(area)) values.Add("area", area);
        }

        private string AreaName(Type controllerType)
        {
            var nameSpace = controllerType.Namespace;
            if (nameSpace == null) return null;

            const string areasStartSearchString = "Areas.";
            var areasIndexOf = nameSpace.IndexOf(areasStartSearchString, StringComparison.Ordinal);
            if (areasIndexOf < 0) return null;
            var areaStart = areasIndexOf + areasStartSearchString.Length;
            var areaString = nameSpace.Substring(areaStart);
            return areaString;
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
				var expectedValue = GetExpectedValue(arguments[i]);
				var isFromBody = attributeRecogniser.IsFromBody(param);

				if (expectedValue != null || !isFromBody)
				{
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
							if (field.Value != null)
							{
								var fieldName = field.Key.ToLowerInvariant();
								if (values.ContainsKey(fieldName))
								{
									string message = string.Format("Duplicate field name: '{0}'", fieldName);
									throw new ApplicationException(message);
								}

								values.Add(fieldName, field.Value);
							}
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