using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace WhatsYourIdea.Web.Tests
{
    public class IntegrationTests
    {
        private readonly TestWebApplication _factory;

        public IntegrationTests()
        {
            _factory = new TestWebApplication();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/?option=new")]
        [InlineData("/?option=popular")]
        [InlineData("/tag/tag_1")]
        [InlineData("/Idea/w2GYVBYRZX")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Act
            var response = await _factory.CreateClient().GetAsync(url);
            
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_SecurePageRequiresAnAuthenticatedUser()
        {

            // Act
            var response = await _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            }).GetAsync("/Admin");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Account/Login",
                               response.Headers.Location.OriginalString);
        }

        [Fact]
        public async Task Get_SecurePageIsAvailableForAuthenticatedUser()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddMvc(options =>
                    {
                        options.Filters.Add(new AllowAnonymousFilter());
                        options.Filters.Add(new FakeUserFilter());
                    })
                    .AddApplicationPart(typeof(Program).Assembly);
                });
            })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync("/editor");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Post_GetSearchPageSuccess()
        {
            var json = JsonSerializer.Serialize(new { search = "Lorem" });
            
            var response = await _factory.CreateClient().PostAsync("/search", new StringContent(json, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}