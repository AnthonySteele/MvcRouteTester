# MvcRouteTester


## What

MvcRouteTester is a .Net library to help unit testing ASP MVC route tables. It contains asserts for for both regular controllers and the new Api controllers. It is build in .Net 4.5 and MVC 4.0.

## Why

To aid automated testing by allowing unit tests on routes. Without such a library, the only way to automate a test that your ASP MVC application responds to a certain route is to make an integration test which fires up the whole MVC application in a web server and issues a Http request to that Url. There's nothing bad about having integration tests, but unit tests run faster, easier to configure, are less fragile and generally come earlier in the coding lifecycle, so there are good reasons to use them as a first line of defence against route configuration errors.

## How


It uses the basic ideas and [code by Phil Haack for testing MVC Routes](http://haacked.com/archive/2007/12/16/testing-routes-in-asp.net-mvc.aspx)
and by [Filip W for testing API Routes](http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/) and puts them together in one convenient package.

It relies on NUnit and Moq. But if you needed to use other equivalent libraries for assertions and mocks, it would probably be not very hard to swap out these dependencies.

## Usage

    RouteAssert.HasRoute(routes, "~/home/index");

This assertion will fail if the route table does not have a route to "/home/index"


    var expectedRoute = new { controller = "Home", action = "Index", id = 42 };
    RouteAssert.HasRoute(routes, "~/home/Index/42", expectedRoute);

This assertion will fail if the route table does not have a route to the url "/home/index/42", and will also fail if it is not mapped to the expected controller, action and other parameters, using a anon typed object to specify the expectations, as per Phil Haack's article. 

There are overloads of this method so if you don't like anonymous types, you can specify a controller name and action name or an IDictionary&lt;string, string&gt; to hold the controller name, action name and any other route parameters.


    RouteAssert.NoRoute(routes, "~/foo/bar/fish/spon");
	
This assertion will fail if the url "/foo/bar/fish/spon" **can** be mapped to a route.
	
### API route usage

    RouteAssert.HasApiRoute(config, "~/api/customer/1", HttpMethod.Get);

This assertion will fail if the config does not contain an Api route to the url "/api/customer/1" which responds to the Http Get method.


    RouteAssert.ApiRouteDoesNotHaveMethod(config, "~/api/customer/1", HttpMethod.Post);

This asserts that the Api route is valid, but that the controller there does not respond to the Http Post method.


    RouteAssert.NoApiRoute(config, "~/pai/customer/1");

This asserts that the api route is not valid.

For more complete test examples, see the self-test code in MvcRouteTester.Test.
