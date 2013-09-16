using System.Net.Http;
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
		public void TestCreateMap()
		{
			var item = new Item { Id = 42 };
			config
				.ShouldMap(string.Format("http://localhost/items/aid"))
				.WithBody("id=42")
				.To<ItemController>(HttpMethod.Post, x => x.CreateItem("aid", item));
		}
	}
}
