using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace MvcRouteTester.AttributeRouting.Test.WebRoute
{
	[TestFixture]
	public class HomeAttrControllerFailTests
	{
		private FakeAssertEngine assertEngine;

		[SetUp]
		public void Setup()
		{
			assertEngine = new FakeAssertEngine();
			RouteAssert.UseAssertEngine(assertEngine);
		}

		[TearDown]
		public void Teardown()
		{
			RouteAssert.UseAssertEngine(new NunitAssertEngine());
		}
	}
}
