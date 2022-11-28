using System.Data.SqlClient;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using RepoDb;
using RepoDb.DbHelpers;
using RepoDb.DbSettings;
using RepoDb.StatementBuilders;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
       
        public static IApplicationBuilder UseNokates(this IApplicationBuilder app)
        {
            //app.UseRouting();
            var logRequestMetrics = ConfigurationValues.GetBoolValueOrDefault("LogRequestMetrics");
            if (logRequestMetrics)
            app.UseMiddleware<RequestMetricsMiddleware>();
            app.UseAuthentication();
           // app.UseAuthorization();
            app.UseMiddleware<CustomExceptionMiddleware>();
            app.UseMiddleware<AuthorizationMiddleware>();

            return app;
        }

        
    }
}
