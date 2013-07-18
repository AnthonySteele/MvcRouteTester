using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
    public class UriDataModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public int? OtherNumber { get; set; }
    }

    public class FromUriController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage DoSomething([FromUri] UriDataModel data)
        {
            return new HttpResponseMessage();
        }
    }
}
