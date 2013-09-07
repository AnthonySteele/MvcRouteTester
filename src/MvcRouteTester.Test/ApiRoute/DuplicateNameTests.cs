using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Routing;

using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
	public class Item
	{
		public int Id { get; set; }
	}

	public class ItemController : ApiController
	{
		[HttpPost]
		public void CreateItem(string id, [FromBody] Item item)
		{
		}
	}

	[TestFixture]
	public class DuplicateNameTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void MakeRouteTable()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());

			config = new HttpConfiguration();
			config.Routes.MapHttpRoute("Create Item", "items/{id}", 
				new { controller = "Item", action = "CreateItem" },
				new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
		}

		[Test]
		[Ignore("has duplicate route param 'id', not working yet")]
		public void TestCreateMap()
		{
			var item = new Item { Id = 42 };
			config
				.ShouldMap(string.Format("http://localhost/items/aid"))
				.WithBody(HttpUtility.UrlEncode(Json.Encode(item)))
				.To<ItemController>(HttpMethod.Post, x => x.CreateItem("aid", item));
		}
	}
}
