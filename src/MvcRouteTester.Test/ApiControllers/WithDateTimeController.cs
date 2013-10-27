using System;
using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	public class DateTimeInputModel
	{
		public DateTime Id { get; set; }
	}
	
	public class WithDateTimeController : ApiController
	{
		public HttpResponseMessage Get(DateTimeInputModel model)
		{
			return new HttpResponseMessage();
		}
	}
}
