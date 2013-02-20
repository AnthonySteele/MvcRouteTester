using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace MvcRouteTester.Fluent
{
	public class AttributeRecogniser
	{
		public bool IsFromBody(ParameterInfo parameter)
		{
			return parameter.CustomAttributes.Any(
				a => a.AttributeType == typeof(FromBodyAttribute));
		}
	}
}
