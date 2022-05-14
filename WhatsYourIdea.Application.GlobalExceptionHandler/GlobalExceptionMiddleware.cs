using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using System.Text.Json;

namespace GlobalExceptionHandler
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app, JsonSerializerOptions jsonSerializerOptions)
        {
            app.UseMiddleware<GlobalExceptionMiddleware>(jsonSerializerOptions);
            return app;
        }
    }

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public GlobalExceptionMiddleware(RequestDelegate next, JsonSerializerOptions jsonSerializerOptions)
        {
            _next = next;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (GlobalException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.Redirect($"/Error/{ex.StatusCode}");
            }
        }
    }
}