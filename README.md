
This is the practise code for "Implementing Advanced RESTful Concerns with ASP.NET Core 3".

Course can be found at: https://app.pluralsight.com/library/courses/asp-dot-net-core-3-advanced-restful-concerns/table-of-contents

The repo contains code that demostrates:
1. Pagination, sorting, data shaping.
2. HATEOAS.
3. Advanced content negotiation.
4. Support HTTP caching.
5. Support concurrency.

Tips on caching:
Don't implementation cache store in your app. Just implement Etags and validation model in the app. Set the cache header correctly so the external cache server can cache correctly. CDN services are often preferred as an alternative for setting up your owner caching servers.

Other repos:
https://github.com/KevinDockx/BuildingRESTfulAPIAspNetCore3
https://github.com/KevinDockx/ImplementingAdvancedRESTfulConcernsAspNetCore3
https://github.com/KevinDockx/SecuringAspNetCore3WithOAuth2AndOIDC
