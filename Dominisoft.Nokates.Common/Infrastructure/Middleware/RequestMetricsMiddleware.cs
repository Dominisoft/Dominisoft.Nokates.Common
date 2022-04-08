using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Dominisoft.Nokates.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Middleware
{
    public class RequestMetricsMiddleware
    {
        
        private readonly RequestDelegate _next;

        public RequestMetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string request;

            var start = DateTime.Now;

            if (context.Request.HasFormContentType)
            {
                request = await FormatRequestForm(context.Request);
            }
            //First, get the incoming request
            else request = await FormatRequest(context.Request);

            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;

            //Create a new memory stream...
            await using var responseBody = new MemoryStream();
            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            var requestTrackingId = Guid.NewGuid();


            if (context.Request.Headers.ContainsKey("RequestTrackingId")
                && Guid.TryParse(context.Request.Headers["RequestTrackingId"], out var id))
                requestTrackingId = id;


            var requestSource = context.Request.Headers.ContainsKey("RequestTrackingSource")?
                 context.Request.Headers["RequestTrackingSource"][0] : "External";
            
            var remoteIpAddress = context.Connection.RemoteIpAddress;



            Thread.CurrentThread.SetRequestId(requestTrackingId);

            //Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);

            //Format the response from the server
            var response = await FormatResponse(context.Response);

            var responseTime = DateTime.Now - start;

            var endpointAuthDetails = context.Items.ContainsKey("EndpointAuthorizationDetails") ? (EndpointDescription)context.Items["EndpointAuthorizationDetails"] : null;
            var designation = endpointAuthDetails==null?"":$"{AppHelper.GetAppName()}:{endpointAuthDetails.Action}";
            var logRecord = new RequestMetric
            {
                RequestTrackingId = requestTrackingId,
                RequestSource = requestSource,
                RemoteIp = remoteIpAddress?.ToString(),
                RequestJson = request,
                ServiceName = AppHelper.GetAppName(),
                RequestPath = context.Request.Path,
                RequestType = context.Request.Method,
                ResponseCode = context.Response.StatusCode,
                ResponseJson = response,
                RequestStart = start,
                ResponseTime = (long) responseTime.TotalMilliseconds,
                EndpointDesignation = designation
            };



            //Save log to chosen datastore
            LogTransaction(logRecord);

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);

        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            //var body = request.Body;

            //This line allows us to set the reader for the request back at the beginning of its stream.
            request.EnableBuffering();
           // var headers = request.Headers.Aggregate(string.Empty, (current, keyValuePair) => current + $"'{keyValuePair.Key}' : '{keyValuePair.Value}'\r\n");
            //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            var stream = new MemoryStream(buffer);
            //...Then we copy the entire request stream into the new buffer.
            await request.Body.CopyToAsync(stream);

            //We convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;


            //..and finally, assign the read body back to the request body, which is allowed because of EnableBuffering()



            var requestString =  JsonConvert.SerializeObject(new
                {
                    request.Method,
                    request.Scheme,
                    request.Host,
                    request.Path,
                    request.QueryString,
                    request.Headers,
                    request.ContentType,
                    SourceIp = remoteIpAddress?.ToString(),
                    bodyAsText,

                },
                Formatting.Indented);

            request.Body.Position = 0;

            return requestString;

        }
        private async Task<string> FormatRequestForm(HttpRequest request)
        {
            //This line allows us to set the reader for the request back at the beginning of its stream.
            //request.EnableRewind();

            //...Then we copy the entire request stream into the new buffer.
            var formData = request.Form;

            var headers = request.Headers.Aggregate(string.Empty, (current, keyValuePair) => current + $"'{keyValuePair.Key}' : '{keyValuePair.Value}'\r\n");

            var bodyAsText = formData.Aggregate(string.Empty, (current, keyValuePair) => current + $"'{keyValuePair.Key}' : '{keyValuePair.Value}'\r\n");

            bodyAsText = formData.Files.Aggregate(bodyAsText, (current, formDataFile) => current + $"File:{formDataFile.FileName}  : {formDataFile.Length} bytes\r\n");

            //..and finally, assign the read body back to the request body, which is allowed because of EnableRewind()
            request.Form = new FormCollection(new Dictionary<string, StringValues>(formData), formData.Files);

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString}\r\nHeaders:\r\n{headers}\r\nBody:\r\n {bodyAsText}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);

            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();

            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);
            
            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return text;
        }

        private void LogTransaction(RequestMetric request) 
            => StatusValues.LogRequestAndResponse(request);
    }
}
