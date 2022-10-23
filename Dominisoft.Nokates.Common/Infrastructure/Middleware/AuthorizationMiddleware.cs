using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.CustomExceptions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Dominisoft.Nokates.Common.Infrastructure.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly List<EndpointDataSource> _endpointSources;

        public AuthorizationMiddleware(RequestDelegate next, IEnumerable<EndpointDataSource> endpointSources)
        {
            _next = next;
            _endpointSources = endpointSources.ToList();
        }


        public async Task Invoke(HttpContext context)
        {

            var endpoint = GetEndpoint(context);

            if (endpoint == null)
            {
                ConfigurationValues.TryGetValue(out bool allowPassthroughOnUnknownEndpoints, "AllowPassthroughOnUnknownEndpoints");

                if (allowPassthroughOnUnknownEndpoints)
                {
                    await _next(context);
                    return;
                }
                context.Response.StatusCode = 502;
                await context.Response.WriteAsync($"Unable to find Endpoint \"{context.Request.Path}\" for authorization");
                return;
            }

            context.Items.Add("EndpointAuthorizationDetails", endpoint);

            if (endpoint.HasNoAuthAttribute)
            {
                await _next(context);
                return;
            }

            var endpointKey = $"{AppHelper.GetAppName()}:{endpoint.Action}";

            CheckAccess(context, endpointKey);

            await _next(context);                
        }

        private EndpointDescription GetEndpoint(HttpContext context)
        {
            var endpoint = AppHelper.GetEndpoint(_endpointSources,$"{context.Request.Method}:{context.Request.Path}");

            return endpoint;
        }

        private static void CheckAccess(HttpContext context, string permission)
        {
           
            var claims = context.User.Claims.ToList();

            if (claims.Any(claim => claim.Type == "role_name" && claim.Value.Trim() == "Admin"))
                return;
            var claimsString = string.Join(",", claims.Select(claim => $"{claim.Type}:{claim.Value}"));
            const string key = "endpoint_permission";

            var effectivePermissions = claims.Where(claim => claim.Type == key).Select(c => c.Value).ToList();           

            var hasAccess= effectivePermissions.Contains(permission) ;
           
            if (hasAccess) return;

            throw new AuthorizationException($"User does not have permission for endpoint {permission}\r\nClaims:{claimsString}");
 
        }
    }
}
