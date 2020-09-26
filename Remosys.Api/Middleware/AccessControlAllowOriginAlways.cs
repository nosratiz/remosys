using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Remosys.Api.Middleware
{
    public class AccessControlAllowOriginAlways
    {
        private readonly RequestDelegate _next;
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        public AccessControlAllowOriginAlways(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                if (!context.Response.Headers.ContainsKey(AccessControlAllowOrigin))
                {
                    context.Response.Headers.Add(AccessControlAllowOrigin, "*");
                }
                return Task.CompletedTask;
            });

            return _next(context);
        }
    }

    public static class AccessControlAllowOriginAlwaysExtensions
    {
        public static IApplicationBuilder UseAccessControlAllowOriginAlways(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessControlAllowOriginAlways>();
        }
    }
}