using Microsoft.AspNetCore.Builder;
using SmartWallit.MIddleware;

namespace SmartWallit.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
