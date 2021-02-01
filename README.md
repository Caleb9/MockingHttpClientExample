A quick example of swapping `HttpClient` in integration tests for
ASP.NET Core application using built-in DI container. By integration
tests I mean tests using the `Microsoft.AspNetCore.Mvc.Testing`
library.

Basically it boils down to:

1. Using `HttpClient` in an explicit service class. When injecting
   `HttpClient` directly into an ASP.NET Core controller class,
   replacing the client does not work.
2. Over-registering `HttpClient` for the service in tests with
   `.AddHttpMessageHandler<T>()` extension method. The actual test
   double must be implementing a `DelegatingHandler` - NOT an
   `HttpClient`.

This technique can be quite useful in integration testing. This
example serves as a memo to myself how to achieve this.
