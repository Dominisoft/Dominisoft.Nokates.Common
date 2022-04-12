﻿using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
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

        public static string Get(string path)
            => Get(path, _token);
        public static string Get(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Get, token));

        public static TResponse Get<TResponse>(string path)
      => Get<TResponse>(path, _token);
        public static TResponse Get<TResponse>(string path, string token)
            => SendRequestWithReturn<TResponse>(GenerateRequest(path, HttpMethod.Get, token));

        #endregion

        #region POST

        public static string Post(string path)
            => Post(path, _token);
        public static string Post(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Post, token));
        public static string Post(string path, object body)
            => Post(path, body, _token);
        public static string Post(string path, object body, string token)
            => SendRequestWithStringReturn(GenerateRequestWithBody(path, body, HttpMethod.Post, token));
        public static TResponse Post<TResponse>(string path)
            => Post<TResponse>(path, _token);
        public static TResponse Post<TResponse>(string path, string token)
            => SendRequestWithReturn<TResponse>(GenerateRequest(path, HttpMethod.Post, token));
        public static TResponse Post<TResponse>(string path, object body)
            => Post<TResponse>(path, body, _token);
        public static TResponse Post<TResponse>(string path, object body, string token)
            => SendRequestWithReturn<TResponse>(GenerateRequestWithBody(path, body, HttpMethod.Post, token));

        #endregion

        #region PUT

        public static string Put(string path)
            => Put(path, _token);
        public static string Put(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Put, token));
        public static string Put(string path, object body)
            => Put(path, body, _token);
        public static string Put(string path, object body, string token)
            => SendRequestWithStringReturn(GenerateRequestWithBody(path, body, HttpMethod.Put, token));
        public static TResponse Put<TResponse>(string path)
            => Put<TResponse>(path, _token);
        public static TResponse Put<TResponse>(string path, string token)
            => SendRequestWithReturn<TResponse>(GenerateRequest(path, HttpMethod.Put, token));
        public static TResponse Put<TResponse>(string path, object body)
            => Put<TResponse>(path, body, _token);
        public static TResponse Put<TResponse>(string path, object body, string token)
            => SendRequestWithReturn<TResponse>(GenerateRequestWithBody(path, body, HttpMethod.Put, token));

        #endregion

        #region Delete

        public static string Delete(string path)
            => Delete(path, _token);
        public static string Delete(string path, string token)
            => SendRequestWithStringReturn(GenerateRequest(path, HttpMethod.Delete, token));
        public static string Delete(string path, object body)
            => Delete(path, body, _token);
        public static string Delete(string path, object body, string token)
            => SendRequestWithStringReturn(GenerateRequestWithBody(path, body, HttpMethod.Delete, token));
        public static TResponse Delete<TResponse>(string path)
            => Delete<TResponse>(path, _token);
        public static TResponse Delete<TResponse>(string path, string token)
            => SendRequestWithReturn<TResponse>(GenerateRequest(path, HttpMethod.Delete, token));
        public static TResponse Delete<TResponse>(string path, object body)
            => Delete<TResponse>(path, body, _token);
        public static TResponse Delete<TResponse>(string path, object body, string token)
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

        public static TResponse SendRequestWithReturn<TResponse>(HttpRequestMessage request)
            => JsonConvert.DeserializeObject<TResponse>(SendRequestWithStringReturn(request));
        public static string SendRequestWithStringReturn(HttpRequestMessage request)
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
            return task2.Result;
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

