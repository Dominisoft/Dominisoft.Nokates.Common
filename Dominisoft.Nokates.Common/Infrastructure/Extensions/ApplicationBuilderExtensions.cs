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

            SetupRepoDb();

            return app;
        }

        private static void SetupRepoDb()
        {
            SqlServerBootstrap.Initialize();

            var dbSetting = new SqlServerDbSetting();

            DbSettingMapper
                .Add<SqlConnection>(dbSetting, true);
            DbHelperMapper
                .Add<SqlConnection>(new SqlServerDbHelper(), true);
            StatementBuilderMapper
                .Add<SqlConnection>(new SqlServerStatementBuilder(dbSetting), true);
        }
    }
}
