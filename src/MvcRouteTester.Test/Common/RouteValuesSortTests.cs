using System.Collections.Generic;
using System.Web.Http;

using MvcRouteTester.Common;

using NUnit.Framework;

namespace MvcRouteTester.Test.Common
{
	[TestFixture]
	public class RouteValuesSortTests
	{
		[Test]
		public void ShouldKeepValuesSortedAlphabetically()
		{
			var valuesIn = new Dictionary<string, string>
				{
					 { "controller", "foo" },
					 { "action", "bar" },
					 { "a", "a" },
					 { "b", "b" },
					 { "z", "z" }
				};

			var routeValues = new RouteValues(valuesIn);

			routeValues.Sort();

			Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
			Assert.That(routeValues.Values[1].Name, Is.EqualTo("b"));
			Assert.That(routeValues.Values[2].Name, Is.EqualTo("z"));
		}

		[Test]
		public void ShouldSortValuesAlphabetically()
		{
			var valuesIn = new Dictionary<string, string>
				{
					 { "controller", "foo" },
					 { "action", "bar" },
					 { "z", "z" },
					 { "b", "b" },
					 { "a", "a" }
				};

			var routeValues = new RouteValues(valuesIn);

			routeValues.Sort();

			Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
			Assert.That(routeValues.Values[1].Name, Is.EqualTo("b"));
			Assert.That(routeValues.Values[2].Name, Is.EqualTo("z"));
		}

		[Test]
		public void ShouldSortValuesAlphabeticallyByName()
		{
			var valuesIn = new Dictionary<string, string>
				{
					 { "controller", "foo" },
					 { "action", "bar" },
					 { "z", "a" },
					 { "b", "b" },
					 { "a", "z" }
				};

			var routeValues = new RouteValues(valuesIn);

			routeValues.Sort();

			Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
			Assert.That(routeValues.Values[1].Name, Is.EqualTo("b"));
			Assert.That(routeValues.Values[2].Name, Is.EqualTo("z"));
		}

		[Test]
		public void ShouldPutOptionalRouteValuesinPlace()
		{
			var valuesIn = new Dictionary<string, object>
				{
					 { "controller", "foo" },
					 { "action", "bar" },
					 { "z", "a" },
					 { "b", "b" },
					 { "a", RouteParameter.Optional }
				};

			var routeValues = new RouteValues(valuesIn);

			routeValues.Sort();

			Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
			Assert.That(routeValues.Values[1].Name, Is.EqualTo("b"));
			Assert.That(routeValues.Values[2].Name, Is.EqualTo("z"));
		}

			[Test]
			public void ShouldPutOptionalRouteValuesAfterSameNamedOnes()
			{
				var valuesIn = new Dictionary<string, object>
					{
						 { "controller", "foo" },
						 { "action", "bar" },
						 { "a", RouteParameter.Optional }
					};

				var routeValues = new RouteValues(valuesIn);

				routeValues.Add(new RouteValue("a", "zzz", RouteValueOrigin.Params));

				routeValues.Sort();

				Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
				Assert.That(routeValues.Values[1].Name, Is.EqualTo("a"));
				Assert.That(routeValues.Values[0].Value, Is.EqualTo("zzz"));
				Assert.That(routeValues.Values[1].Value, Is.EqualTo(RouteParameter.Optional));
			}

			[Test]
			public void ShouldPutMultipleOptionalRouteValuesAfterSameNamedOnes()
			{
				var valuesIn = new Dictionary<string, object>
					{
						 { "controller", "foo" },
						 { "action", "bar" },
						 { "a", RouteParameter.Optional },
						 { "b", RouteParameter.Optional },
					};

				var routeValues = new RouteValues(valuesIn);

				routeValues.Add(new RouteValue("a", "zzz", RouteValueOrigin.Params));
				routeValues.Add(new RouteValue("b", "bbb", RouteValueOrigin.Params));

				routeValues.Sort();

				Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
				Assert.That(routeValues.Values[1].Name, Is.EqualTo("a"));
				Assert.That(routeValues.Values[0].Value, Is.EqualTo("zzz"));
				Assert.That(routeValues.Values[1].Value, Is.EqualTo(RouteParameter.Optional));

				Assert.That(routeValues.Values[2].Name, Is.EqualTo("b"));
				Assert.That(routeValues.Values[3].Name, Is.EqualTo("b"));
				Assert.That(routeValues.Values[2].Value, Is.EqualTo("bbb"));
				Assert.That(routeValues.Values[3].Value, Is.EqualTo(RouteParameter.Optional));
			}

			[Test]
			public void SameNamedValuesAreSortedByOrigin()
			{
				var routeValues = new RouteValues();

				routeValues.Add(new RouteValue("a", "ccc", RouteValueOrigin.Params));
				routeValues.Add(new RouteValue("a", "aaa", RouteValueOrigin.Body));
				routeValues.Add(new RouteValue("a", "bbb", RouteValueOrigin.Path));

				routeValues.Sort();

				Assert.That(routeValues.Values[0].Name, Is.EqualTo("a"));
				Assert.That(routeValues.Values[1].Name, Is.EqualTo("a"));
				Assert.That(routeValues.Values[2].Name, Is.EqualTo("a"));

				Assert.That(routeValues.Values[0].Origin, Is.EqualTo(RouteValueOrigin.Path));
				Assert.That(routeValues.Values[1].Origin, Is.EqualTo(RouteValueOrigin.Params));
				Assert.That(routeValues.Values[2].Origin, Is.EqualTo(RouteValueOrigin.Body));
			}
	}
}
