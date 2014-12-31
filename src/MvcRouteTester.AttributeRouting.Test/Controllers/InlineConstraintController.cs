using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcRouteTester.AttributeRouting.Test.Controllers
{
    public class InlineConstraintController : Controller
    {
        [Route("inlineconstraint/{param:verb}")]
		public ActionResult Index(string param)
		{
			return new EmptyResult();
		}
    }

    public class CustomConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            object paramValue;
            values.TryGetValue(parameterName, out paramValue);
            
            return paramValue != null && paramValue.ToString().EndsWith("ing");
        }
    }
}
