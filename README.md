# MvcRouteTester


## What

MvcRouteTester is a .Net library to help unit testing ASP MVC route tables. It contains asserts for for both regular controllers and the Api controllers that are new in MVC 4.0. It is built in .Net 4.5 and ASP MVC 4.0.

## Why

To aid automated testing by allowing unit tests on routes. Without such a library, the only way to automate a test that your ASP MVC application responds to a certain route is to make an integration test which fires up the whole MVC application in a web server and issues a Http request to that Url. Integration tests are good too, but unit tests run faster, easier to configure, are less fragile and generally come earlier in the coding lifecycle, so there are good reasons to use them as a first line of defence against route configuration errors.

## How

It uses the basic ideas and code by [Phil Haack for testing MVC Routes](http://haacked.com/archive/2007/12/16/testing-routes-in-asp.net-mvc.aspx)
and by [Filip W for testing API Routes](http://www.strathweb.com/2012/08/testing-routes-in-asp-net-web-api/). It puts them together in one convenient package. I have told them both about this use of thier code and they are happy to see it here.

It relies on [NUnit](http://www.nunit.org/) and [Moq](http://code.google.com/p/moq/). But if you needed to use other equivalent libraries for assertions and mocks, it should be easy to swap out these dependencies.

## Licence

Copyright 2013 Anthony Steele. This library is Open Source and you are welcome to use it as you see fit. Now the offical wording: 

It is licensed under [the Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html) (the "License");
you may not use library file except in compliance with the License.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

## Where can I get it?

You can get the source from here on GitHub, or [get the binaries from nuget](http://www.nuget.org/packages/MvcRouteTester/).

## Usage

### Web route usage

Assuming that the routes are declared along these lines: 

    routes = new RouteCollection();
    routes.MapRoute(
        name: "Default",
        url: "{controller}/{action}/{id}",
        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

Then:
		
    RouteAssert.HasRoute(routes, "/home/index");

This assertion will fail if the route table does not have a route to `/home/index`


    var expectedRoute = new { controller = "Home", action = "Index", id = 42 };
    RouteAssert.HasRoute(routes, "/home/Index/42", expectedRoute);

This assertion will fail if the route table does not have a route to the url `/home/index/42`, and will also fail if it is not mapped to the expected controller, action and other parameters, using a anon-typed object to specify the expectations, [as per Phil Haack's article](http://haacked.com/archive/2007/12/16/testing-routes-in-asp.net-mvc.aspx). 

There are overloads of this method so if you don't like anonymous types, you can specify a controller name and action name as strings, or an `IDictionary<string, string>` to hold the controller name, action name and any other route parameters.


    RouteAssert.NoRoute(routes, "/foo/bar/fish/spon");
	
This assertion will fail if a route **can** be found for the url `/foo/bar/fish/spon`.
	
### API route usage

Assuming that the routes are declared along these lines: 

    routes = new HttpConfiguration();
    config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
	    defaults: new { id = RouteParameter.Optional });;

Then:

    RouteAssert.HasApiRoute(config, "/api/customer/1", HttpMethod.Get);

This assertion will fail if the config does not contain an Api route to the url `/api/customer/1` which responds to the Http Get method.

    var expectation = new { controller = "Customer", action= "get", id = "1" };
    RouteAssert.HasApiRoute(config, "/api/customer/1", HttpMethod.Get, expectation);

You can assert on the particulars of the api route using the same kinds of expectations as with Mvc routes.


    RouteAssert.ApiRouteDoesNotHaveMethod(config, "/api/customer/1", HttpMethod.Post);

This asserts that the Api route is valid, but that the controller there does not respond to the Http Post method.


    RouteAssert.NoApiRoute(config, "/pai/customer/1");

This asserts that the api route is not valid.

For more complete test examples, see the self-test code in MvcRouteTester.Test.

## API 

All methods are on the RouteAssert class.

### Web routes

    public static void HasRoute(RouteCollection routes, string url)

Test that a web route matching the url exists.

    public static void HasRoute(RouteCollection routes, string url, object expectations)
    public static void HasRoute(RouteCollection routes, string url, string controller, string action)
    public static void HasRoute(RouteCollection routes, string url, IDictionary<string, string> expectedProps)

Test that a web route matching the url exists, and that it meets expectations. The expectations can be given in different ways  - as an anon-typed object, as an IDictionary of names and values, or to just check the controller and action method names.

    public static void NoRoute(RouteCollection routes, string url)
	
Test that a web route matching the url does not exist.

    public static void IsIgnoredRoute(RouteCollection routes, string url)
	
Test that the url is explicitly ignored, e.g. it matches a route added with `routes.IgnoreRoute`

    public static void IsNotIgnoredRoute(RouteCollection routes, string url)
	
Test that the url is not explicitly ignored, e.g. it matches a route which is not an ignored route.


### Api routes

Api routes work a bit differently to web routes. Once an entry in the route table has been matched, the controller must be found to see if it has a method to respond to the given Http method, so there are two different levels of matching.

    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod)

Test that an Api route matching the url exists, and that the controller can respond to the specified Http method.

    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, object expectations)
    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, string controller, string action)
    public static void HasApiRoute(HttpConfiguration config, string url, HttpMethod httpMethod, IDictionary<string, string> expectedProps)

Test that an api route matching the url exists, and that the controller can respond to the specified Http method and meets expectations. The expectations can be given in different ways - as an anon-typed object, as an IDictionary of names and values, or to just check the controller and action method names.

    public static void NoApiRoute(HttpConfiguration config, string url)

Test that an Api route for the url does not exist. This means that it either does not match any pattern in the route table, or it does match a route table entry, but a matching controller cannot be found.

    public static void ApiRouteMatches(HttpConfiguration config, string url)

Test that an api route matching the url exists. This is a weaker test as it does not attempt to locate the controller, just tests that the url matches a route's pattern.

    public static void NoApiRouteMatches(HttpConfiguration config, string url)

Test that an api route matching the url does not exist. This means that it does not match the pattern of any entry in the route table.
