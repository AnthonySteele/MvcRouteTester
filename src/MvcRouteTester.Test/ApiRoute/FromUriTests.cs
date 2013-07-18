using System.Net.Http;
using System.Web.Http;

using MvcRouteTester.Test.ApiControllers;
using MvcRouteTester.Test.Assertions;

using NUnit.Framework;

namespace MvcRouteTester.Test.ApiRoute
{
    [TestFixture]
    public class FromUriTests
    {
        private HttpConfiguration config;

        [SetUp]
        public void MakeRouteTable()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());

            config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }

        [TearDown]
        public void TearDown()
        {
            RouteAssert.UseAssertEngine(new NunitAssertEngine());
        }

        [Test]
        public void TestHasApiRoute()
        {
            var expectations = new
                {
                    controller = "FromUri",
                    action = "DoSomething"
                };

            RouteAssert.HasApiRoute(config, "/api/fromuri/123", HttpMethod.Get, expectations);
        }

        [Test]
        public void TestHasApiRouteValuesFromUri()
        {
            var expectations = new
            {
                controller = "FromUri",
                action = "DoSomething",
                name = "Fred",
                number = 42
            };

            RouteAssert.HasApiRoute(config, "/api/fromuri?name=Fred&number=42", HttpMethod.Get, expectations);
        }

        [Test]
        public void MismatchFailsValuesFromBody()
        {
            var expectations = new
            {
                controller = "FromUri",
                action = "DoSomething",
                name = "Jim",
                number = 42
            };

            var assertEngine = new FakeAssertEngine();
            RouteAssert.UseAssertEngine(assertEngine);

            RouteAssert.HasApiRoute(config, "/api/fromuri?name=Fred&number=42", HttpMethod.Get, expectations);

            Assert.That(assertEngine.StringMismatchCount, Is.EqualTo(1));
            Assert.That(assertEngine.Messages[0], Is.EqualTo("Expected 'Jim', not 'Fred' for 'name' at url '/api/fromuri?name=Fred&number=42'."));
        }

        [Test]
        public void TestFluentMap()
		{
            config.ShouldMap("/api/fromuri?name=Fred&number=42").
                To<FromUriController>(HttpMethod.Get, c => c.DoSomething(new UriDataModel {Name = "Fred", Number = 42}));
		}

        [Test]
        public void TestFluentMapWithNullablePropertyFilled()
        {
            config.ShouldMap("/api/fromuri?name=Fred&number=42&othernumber=123").
                To<FromUriController>(HttpMethod.Get, c => c.DoSomething(new UriDataModel { Name = "Fred", Number = 42, OtherNumber = 123}));
        }
    }
}
