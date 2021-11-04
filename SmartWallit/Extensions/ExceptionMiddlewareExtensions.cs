using Microsoft.AspNetCore.Builder;
using SmartWallit.Core.MIddleware;
using System;
using System.Collections.Generic;
using System.Text;

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
