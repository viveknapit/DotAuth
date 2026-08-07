using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Presentation.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDotAuthExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();
        }
    }
}
