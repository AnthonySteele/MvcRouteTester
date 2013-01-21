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

## Usage

### Web route usage


    RouteAssert.HasRoute(routes, "/home/index");

This assertion will fail if the route table does not have a route to "/home/index"


    var expectedRoute = new { controller = "Home", action = "Index", id = 42 };
    RouteAssert.HasRoute(routes, "/home/Index/42", expectedRoute);

This assertion will fail if the route table does not have a route to the url "/home/index/42", and will also fail if it is not mapped to the expected controller, action and other parameters, using a anon typed object to specify the expectations, as per Phil Haack's article. 

There are overloads of this method so if you don't like anonymous types, you can specify a controller name and action name as strings, or an IDictionary&lt;string, string&gt; to hold the controller name, action name and any other route parameters.


    RouteAssert.NoRoute(routes, "/foo/bar/fish/spon");
	
This assertion will fail if the url "/foo/bar/fish/spon" **can** be mapped to a route.
	
### API route usage

    RouteAssert.HasApiRoute(config, "/api/customer/1", HttpMethod.Get);

This assertion will fail if the config does not contain an Api route to the url "/api/customer/1" which responds to the Http Get method.

    var expectation = new { controller = "Customer", action= "get", id = "1" };
    RouteAssert.HasApiRoute(config, "/api/customer/1", HttpMethod.Get, expectation);

You can assert on the particulars of the api route using the same kinds of expectations as with Mvc routes.


    RouteAssert.ApiRouteDoesNotHaveMethod(config, "/api/customer/1", HttpMethod.Post);

This asserts that the Api route is valid, but that the controller there does not respond to the Http Post method.


    RouteAssert.NoApiRoute(config, "/pai/customer/1");

This asserts that the api route is not valid.

For more complete test examples, see the self-test code in MvcRouteTester.Test.
