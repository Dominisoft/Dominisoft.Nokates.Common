using System.Data.SqlClient;
using Dominisoft.Nokates.Common.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
       
        public static IApplicationBuilder UseNokates(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseMiddleware<RequestMetricsMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();



            return app;
        }
    }
}
