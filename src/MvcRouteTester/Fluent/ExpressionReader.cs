using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using MvcRouteTester.Common;

namespace MvcRouteTester.Fluent
{
	public class ExpressionReader
	{
		public RouteValues Read<TController>(Expression<Func<TController, object>> action)
		{
			return Read(typeof(TController), UnwrapAction(action));
		}

		public RouteValues Read<TController>(Expression<Func<TController, Task<ActionResult>>> action)
		{
			return Read(typeof(TController), UnwrapAction(action));
		}

		public RouteValues Read<TController>(Expression<Func<TController, ActionResult>> action)
		{
			return Read(typeof(TController), UnwrapAction(action));
		}

		public RouteValues Read<TController>(Expression<Action<TController>> action)
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

		private RouteValues Read(Type controllerType, MethodCallExpression methodCall)
		{
			var values = new RouteValues();
			values.Controller = ControllerName(controllerType);
			values.Action = ActionName(methodCall);
			values.Area = AreaName(controllerType);

			values.AddRange(ReadParameters(methodCall));

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

		private string AreaName(Type controllerType)
		{
			var routeAreaAttributes = controllerType.GetCustomAttributes(typeof(RouteAreaAttribute), true);
			if (routeAreaAttributes.Length > 0)
			{
				var routeArea = (RouteAreaAttribute)(routeAreaAttributes[0]);
				return routeArea.AreaName;
			}


			var nameSpace = controllerType.Namespace;
			if (nameSpace == null)
			{
				return string.Empty;
			}

			const string AreasStartSearchString = "Areas.";
			var areasIndexOf = nameSpace.IndexOf(AreasStartSearchString, StringComparison.Ordinal);
			if (areasIndexOf < 0)
			{
				return string.Empty;
			}

			var areaStart = areasIndexOf + AreasStartSearchString.Length;
			var areaString = nameSpace.Substring(areaStart);
			if (areaString.Contains("."))
			{
				areaString = areaString.Remove(areaString.IndexOf(".", StringComparison.Ordinal));
			}
			return areaString;
		}

		private string ActionName(MethodCallExpression methodCall)
		{
			// Look for System.Web.Mvc ActionName attribute
			var attributes = methodCall.Method.GetCustomAttributes(typeof(System.Web.Mvc.ActionNameAttribute), true);

			if (attributes.Length > 0)
			{
				return ((ActionNameAttribute)attributes[0]).Name;
			}

			// Look for System.Web.Http ActionName attribute
			attributes = methodCall.Method.GetCustomAttributes(typeof(System.Web.Http.ActionNameAttribute), true);

			if (attributes.Length > 0)
			{
				return ((System.Web.Http.ActionNameAttribute)attributes[0]).Name;
			}

			// Otherwise, just use the method name
			return methodCall.Method.Name;
		}

		private IList<RouteValue> ReadParameters(MethodCallExpression methodCall)
		{
			var values = new List<RouteValue>();

			var attributeRecogniser = new AttributeRecogniser();
			var propertyReader = new PropertyReader();

			var parameters = methodCall.Method.GetParameters();
			var arguments = methodCall.Arguments;

			for (int i = 0; i < parameters.Length; i++)
			{
				var param = parameters[i];
				var expectedValue = GetExpectedValue(arguments[i]);
				var isFromBody = attributeRecogniser.IsFromBody(param);
				var routeValueOrigin = isFromBody ? RouteValueOrigin.Body : RouteValueOrigin.Unknown;
				if (expectedValue != null || !isFromBody)
				{
					if (propertyReader.IsSimpleType(param.ParameterType))
					{
						var resultValue = new RouteValue(param.Name, expectedValue, routeValueOrigin);
						values.Add(resultValue);
					}
					else
					{
						var objectFieldValues = propertyReader.PropertiesList(expectedValue, routeValueOrigin)
							.Where(x => x.Value != null);
						values.AddRange(objectFieldValues);
					}
				}
			}

			return values;
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