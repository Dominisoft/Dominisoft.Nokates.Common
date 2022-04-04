using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using Newtonsoft.Json;

namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class HttpHelper
    {
        public static TReturn Get<TReturn>(string path, string token)
        {
            var json = "";
            var client = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(path),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("Authorization", $"Bearer {token}");
            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new AuthenticationException($"Unable to authenticate to endpoint {path}");
                    json = response.Content.ReadAsStringAsync().Result;
                });
            task.Wait();
            return JsonConvert.DeserializeObject<TReturn>(json);
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
            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                    json = response.Content.ReadAsStringAsync().Result;
                });
            task.Wait();
            return JsonConvert.DeserializeObject<TReturn>(json);
        }

        public static void Post(string path, string body, string token)
        {
            var client = new HttpClient();
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(path),
                Method = HttpMethod.Post,
                Content = content,

            };
            request.Headers.Add("Authorization", token);
            var task = client.SendAsync(request)
                .ContinueWith((taskwithmsg) =>
                {
                    var response = taskwithmsg.Result;
                });
            task.Wait();
        }
    }
}
