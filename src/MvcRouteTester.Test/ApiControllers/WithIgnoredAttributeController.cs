using System;
using System.Net.Http;
using System.Web.Http;

namespace MvcRouteTester.Test.ApiControllers
{
	internal class IgnoreMeAttribute : Attribute
	{
	}

	public class TestModel
	{
		[IgnoreMe]
		public string Ignored { get; set; }

		public int Id { get; set; }
	}

	public class WithIgnoredAttributeController : ApiController
	{
		public HttpResponseMessage Get(TestModel model)
		{
			return new HttpResponseMessage();
		}
	}
}
