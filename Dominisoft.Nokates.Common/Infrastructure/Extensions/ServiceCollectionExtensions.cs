using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Controllers;
using Dominisoft.Nokates.Common.Infrastructure.Conventions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Infrastructure.Listener;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Dominisoft.Nokates.Common.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNokates(this IServiceCollection services, string configurationAppName = "Configuration", string configFile = null)
        {
            if (string.IsNullOrWhiteSpace(configFile))
            {
                ConfigurationValues.LoadConfig(configurationAppName).Wait();
                StatusValues.Log("Configuration Downloaded: " + ConfigurationValues.JsonConfig);

            }
            else
            {
                ConfigurationValues.LoadConfigFromFile(configFile);
                StatusValues.Log("Configuration loaded from file: " + ConfigurationValues.JsonConfig);
            }
            EventRecorder.Start();

            services.SetupJwtServices();

            var mvc = services.AddMvcCore(options =>
            {
                options.Conventions.Add(new ApiExplorerVisibilityEnabledConvention());
            }
            );

            mvc.AddApplicationPart(typeof(StatusController).Assembly);
            StatusValues.Status = new ServiceStatus
            {
                IsOnline = true,
                StartTime = DateTime.Now,
                Name = AppHelper.GetAppName(),
                Uri = AppHelper.GetAppUri() + "ServiceStatus",
                Version = AppHelper.GetVersionDetails()
            };
            StatusValues.Log("Nokates Service configured");


            return services;
        }
        private static void SetupJwtServices(this IServiceCollection services)
        {
            //TODO: This should come from config
            var key = "my_secret_key_12345"; //this should be same which is used while creating token    
            var issuer = "http://mysite.com";  //this should be same which is used while creating token


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddAuthorization();
        }
    }
}
