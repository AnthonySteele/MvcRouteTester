using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace MvcRouteTester.Fluent
{
	public class AttributeRecogniser
	{
		public bool IsFromBody(ParameterInfo parameter)
		{
			return parameter.GetCustomAttributes(false).Any(a => a is FromBodyAttribute);
		}
	}
}
