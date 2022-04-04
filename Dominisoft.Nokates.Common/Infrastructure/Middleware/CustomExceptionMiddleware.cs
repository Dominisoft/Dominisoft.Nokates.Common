using System;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace Dominisoft.Nokates.Common.Infrastructure.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
                return;
            }
            catch(RequestException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.ToString());
            }

        }
    }
}
