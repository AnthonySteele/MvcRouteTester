# MvcRouteTester

## What

MvcRouteTester is a .Net library to help unit testing ASP MVC route tables. It contains asserts for for both regular controllers and the Api controllers that are new in MVC 4.0. It is built in .Net 4.0 and [ASP MVC 4.0](http://www.nuget.org/packages/MvcRouteTester/) or [ASP MVC 5.0](http://www.nuget.org/packages/MvcRouteTester.MVC5/).

It can be used with a simple or a fluent syntax, e.g either `RouteAssert.HasRoute(routes, "/home/index");` or `routes.ShouldMap("/home/index").To<HomeController>(x => x.Index());`

## Why

To aid automated testing by allowing unit tests on routes. Without such a library, the only way to automate a test that your ASP MVC application responds to a certain route is to make an integration test which fires up the whole MVC application in a web server and issues a Http request to that URL. Integration tests are good too, but unit tests run faster, easier to configure, are less fragile and generally come earlier in the coding life-cycle, so there are good reasons to use unit tests as a first line of defence against route configuration errors.

## How

MvcRouteTester throws an exception in order to fail a test on a route. This should work in any unit testing framework. However, if you want to integrate closer with NUnit or another unit testing framework, [you can do this](https://github.com/AnthonySteele/MvcRouteTester/wiki/Integrating-with-NUnit).
****
## Licence

Copyright 2013 Anthony Steele. This library is Open Source and you are welcome to use it as you see fit. Now the official wording: 

It is licensed under [the Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0.html) (the "License");
you may not use library file except in compliance with the License.

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

## Where can I get it?

**[There is a package for ASP MVC 5.1](http://www.nuget.org/packages/MvcRouteTester.Mvc5.1/)** on [NuGet.org](http://www.nuget.org/). There are also packages for previous framework versions: [for ASP MVC 5.0](http://www.nuget.org/packages/MvcRouteTester.MVC5/) or  [for ASP MVC 4.0](http://www.nuget.org/packages/MvcRouteTester/)

You can get or fork the source here on GitHub. The master branch is used for ASP MVC 5.1 as this is the latest version, and there are branches [for MVC 5.0](https://github.com/AnthonySteele/MvcRouteTester/tree/MVC5) and [for MVC 4](https://github.com/AnthonySteele/MvcRouteTester/tree/MVC4).

## Other pages

 - [Credits](https://github.com/AnthonySteele/MvcRouteTester/wiki/Credits)
 - [More detailed usage documentation](https://github.com/AnthonySteele/MvcRouteTester/wiki/Usage)
 - [Integrating with NUnit](https://github.com/AnthonySteele/MvcRouteTester/wiki/Integrating-with-NUnit)

