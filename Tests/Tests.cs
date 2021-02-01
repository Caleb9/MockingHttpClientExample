using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tests
{
    public class Tests :
        IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public Tests(
            WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Example_of_replacing_HttpClient_with_test_double()
        {
            using var client =
                _factory
                    .WithWebHostBuilder(builder =>
                        builder.ConfigureServices(services =>
                            services
                                /* Replacement has to be registered for AddHttpMessageHandler to work. */
                                .AddSingleton<TestDelegatingHandler>()
                                /* A Service needs to exist. Injecting HttpClient directly into a controller makes it
                                 * impossible to swap the handler. */
                                .AddHttpClient<HttpClientIsolationService>()
                                /* Finally we replace the handler. */
                                .AddHttpMessageHandler<TestDelegatingHandler>()))
                    .CreateDefaultClient();

            var response = await client.GetAsync("/hello");

            response.IsSuccessStatusCode.Should().BeTrue();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("testing result");
        }

        /* Usage of mocking library is not impossible, although more difficult because SendAsync is a *protected*
         * method. When using manually coded test double like here in a real test, some way of setting it up should
         * also be implemented. */
        private class TestDelegatingHandler :
            DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("testing result")
                    });
            }
        }
    }
}