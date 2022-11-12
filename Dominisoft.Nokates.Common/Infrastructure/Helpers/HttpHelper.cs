using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Models;
using Newtonsoft.Json;


namespace Dominisoft.Nokates.Common.Infrastructure.Helpers
{
    public static class HttpHelper
    {
        #region Token

        private static string _token;
        public static void SetToken(string token)
            => _token = token;

        #endregion

        #region GET

        public static RestResponse Get(string path)
            => Get(path, _token);
        public static RestResponse Get(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Get, token));

        public static RestResponse<TResponse> Get<TResponse>(string path) where TResponse : class 
            => Get<TResponse>(path, _token);
        public static RestResponse<TResponse> Get<TResponse>(string path, string token) where TResponse : class
            => SendRequestWithObjectReturn<TResponse>(GenerateRequest(path, HttpMethod.Get, token));

        #endregion

        #region POST

        public static RestResponse Post(string path)
            => Post(path, _token);
        public static RestResponse Post(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Post, token));
        public static RestResponse Post(string path, object body)
            => Post(path, body, _token);
        public static RestResponse Post(string path, object body, string token)
            => SendRequestWithStringReturn(GenerateRequestWithBody(path, body, HttpMethod.Post, token));
        public static RestResponse<TResponse> Post<TResponse>(string path) where TResponse : class 
            => Post<TResponse>(path, _token);
        public static RestResponse<TResponse> Post<TResponse>(string path, string token) where TResponse : class
            => SendRequestWithObjectReturn<TResponse>(GenerateRequest(path, HttpMethod.Post, token));
        public static RestResponse<TResponse> Post<TResponse>(string path, object body) where TResponse : class
            => Post<TResponse>(path, body, _token);
        public static RestResponse<TResponse> Post<TResponse>(string path, object body, string token) where TResponse : class
            => SendRequestWithObjectReturn<TResponse>(GenerateRequestWithBody(path, body, HttpMethod.Post, token));

        #endregion

        #region PUT

        public static RestResponse Put(string path)
            => Put(path, _token);
        public static RestResponse Put(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Put, token));
        public static RestResponse Put(string path, object body)
            => Put(path, body, _token);
        public static RestResponse Put(string path, object body, string token)
            => SendRequestWithStringReturn(GenerateRequestWithBody(path, body, HttpMethod.Put, token));
        public static RestResponse<TResponse> Put<TResponse>(string path) where TResponse : class
            => Put<TResponse>(path, _token);
        public static RestResponse<TResponse> Put<TResponse>(string path, string token) where TResponse : class
            => SendRequestWithReturn<TResponse>(GenerateRequest(path, HttpMethod.Put, token));
        public static RestResponse<TResponse> Put<TResponse>(string path, object body) where TResponse : class
            => Put<TResponse>(path, body, _token);
        public static RestResponse<TResponse> Put<TResponse>(string path, object body, string token) where TResponse : class
            => SendRequestWithReturn<TResponse>(GenerateRequestWithBody(path, body, HttpMethod.Put, token));

        #endregion

        #region Delete

        public static RestResponse Delete(string path)
            => Delete(path, _token);
        public static RestResponse Delete(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Delete, token));
        public static RestResponse Delete(string path, object body)
            => Delete(path, body, _token);
        public static RestResponse Delete(string path, object body, string token)
            => SendRequestWithStringReturn(GenerateRequestWithBody(path, body, HttpMethod.Delete, token));
        public static RestResponse<TResponse> Delete<TResponse>(string path) where TResponse : class
            => Delete<TResponse>(path, _token);
        public static RestResponse<TResponse> Delete<TResponse>(string path, string token) where TResponse : class
            => SendRequestWithReturn<TResponse>(GenerateRequest(path, HttpMethod.Delete, token));
        public static RestResponse<TResponse> Delete<TResponse>(string path, object body) where TResponse : class
            => Delete<TResponse>(path, body, _token);
        public static RestResponse<TResponse> Delete<TResponse>(string path, object body, string token) where TResponse : class
            => SendRequestWithReturn<TResponse>(GenerateRequestWithBody(path, body, HttpMethod.Delete, token));

        #endregion

        #region BuilderMethods

        private static HttpRequestMessage GenerateRequestWithBody<TBody>(string path, TBody body, HttpMethod method, string token = null)
        {
            var request = GenerateRequest(path, method, token);
            var json = body.Serialize();
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return request;
        }

        private static HttpRequestMessage GenerateRequest(string path, HttpMethod method, string token = null)
        {
            return string.IsNullOrWhiteSpace(token) ?
                new HttpRequestMessage()
                {
                    RequestUri = new Uri(path),
                    Method = method,
                    Headers =
                    {
                    }
                }: 
                new HttpRequestMessage()
                {
                    RequestUri = new Uri(path),
                    Method = method,
                    Headers =
                    {
                        { "Accept", "application/json" },
                        {"Authorization", $"Bearer {token}"}
                    }
                };
        }

        public static RestResponse<TResponse> SendRequestWithReturn<TResponse>(HttpRequestMessage request) where TResponse : class 
            => SendRequestWithObjectReturn<TResponse>(request);
        public static RestResponse SendRequestWithStringReturn(HttpRequestMessage request)
        {
            var client = new HttpClient();
            AddRequestTrackingInfoToRequest(ref request);
            var task = client.SendAsync(request);
            task.Wait();
            var response = task.Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new AuthenticationException($"Unable to authenticate to endpoint {request.RequestUri}");
            var task2 = response.Content.ReadAsStringAsync();
            task2.Wait();
            var result = task2.Result;
            return new RestResponse((int) response.StatusCode, result);
        }

        public static RestResponse<TReturn> SendRequestWithObjectReturn<TReturn>(HttpRequestMessage request) where TReturn : class
        {
            var client = new HttpClient();
            AddRequestTrackingInfoToRequest(ref request);
            var task = client.SendAsync(request);
            task.Wait();
            var response = task.Result;
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new AuthenticationException($"Unable to authenticate to endpoint {request.RequestUri}");
            var task2 = response.Content.ReadAsStringAsync();
            task2.Wait();
            var result = task2.Result;
            return new RestResponse<TReturn>((int)response.StatusCode, result);
        }
        private static void AddRequestTrackingInfoToRequest(ref HttpRequestMessage request)
        {
            var appName = AppHelper.GetAppName();
            var requestId = Thread.CurrentThread.GetRequestId();
            if (requestId == Guid.Empty)
                requestId = Guid.NewGuid();
            request.Headers.Add("RequestTrackingId", requestId.ToString());
            request.Headers.Add("RequestTrackingSource", appName);
        }

        #endregion
    }
}

