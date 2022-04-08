using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class HttpHelper
    {
        public static TReturn Get<TReturn>(string path, string token)
        {
            var json = Get(path,token);
            
            return JsonConvert.DeserializeObject<TReturn>(json);
        }
        public static string Get(string path, string token)
        {
            var json = "";
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(path),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("Authorization", $"Bearer {token}");
            AddRequestTrackingInfoToRequest(ref request);
            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new AuthenticationException($"Unable to authenticate to endpoint {path}");
                    json = response.Content.ReadAsStringAsync().Result;
                });
            task.Wait();
            return json;
        }
        public static TReturn Post<TReturn>(string path, string body, string token)
        {
            var json = "";
            var client = new HttpClient();
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(path),
                Method = HttpMethod.Post,
                Content = content,

            };
            request.Headers.Add("Authorization", $"Bearer {token}");
            AddRequestTrackingInfoToRequest(ref request);
            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    json = response.Content.ReadAsStringAsync().Result;
                });
            task.Wait();
            return JsonConvert.DeserializeObject<TReturn>(json);
        }

        public static async Task<string> Post(string path, string body, string token)
        {
            var client = new HttpClient();
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(path),
                Method = HttpMethod.Post,
                Content = content,

            };
            
            if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Add("Authorization", token);

            AddRequestTrackingInfoToRequest(ref request);

            var task = client.SendAsync(request)
                .ContinueWith(taskwithmsg
                    =>
                {
                    var t2 = taskwithmsg.Result.Content.ReadAsStringAsync()
                        .ContinueWith(r => r.Result);
                    t2.Wait();
                    return t2.Result;
                });
            task.Wait();

            string r = task.Result;

            return r;
            //var task = client.SendAsync(request)
            //    .ContinueWith((taskwithmsg) =>
            //    {
            //        var response = taskwithmsg.Result;
            //        return response;
            //    });
            //task.Wait();

            //var task2 = task.Result.Content.ReadAsStringAsync();

            //task2.ContinueWith(r => r);

            //task2.Wait();


            //return task2.Result;
        }

        private static void AddRequestTrackingInfoToRequest(ref HttpRequestMessage request)
        {
            var appName = AppHelper.GetAppName();
            var requestId = Thread.CurrentThread.GetRequestId();
            if (requestId==Guid.Empty) 
                requestId = Guid.NewGuid();
            request.Headers.Add("RequestTrackingId",requestId.ToString() );
            request.Headers.Add("RequestTrackingSource",appName );

        }
    }
}
