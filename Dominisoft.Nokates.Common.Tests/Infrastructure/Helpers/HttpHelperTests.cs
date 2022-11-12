using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Extensions;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.Helpers
{
    [TestFixture]
    public class HttpHelperTests
    {
        private const string EchoUrl = "https://httpbin.org/anything";
        private const string UnauthorizedUrl = "https://httpbin.org/basic-auth/:user/:passwd";
     
        #region GetTests
        [Test]
        public void UnauthorizedRequestShouldThrowException()
        {
            Assert.Throws<AuthenticationException>(() => HttpHelper.Get(UnauthorizedUrl, string.Empty));
        }
        [Test]
        public void GetShouldReturnString()
        {
            var result = HttpHelper.Get(EchoUrl, string.Empty);
            Assert.IsTrue(result.Message.Contains("\"method\": \"GET\""));
        }
        [Test]
        public void GetShouldReturnStringWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Get(EchoUrl);
            var response = result.Message.Deserialize<dynamic>();
            var authHeader = (string)response.headers.Authorization;
            Assert.IsTrue(authHeader.Contains($"Bearer {id}"));
        }
        [Test]
        public void GetShouldMapToObject()
        {
            var result = HttpHelper.Get<TestResponse>(EchoUrl, string.Empty);
            Assert.AreEqual("GET", result.Object.method);
        }

        [Test]
        public void GetShouldAddAuthHeader()
        {
            var id = Guid.NewGuid();
            var result = HttpHelper.Get<TestResponse>(EchoUrl, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void GetShouldAddAuthHeaderFromTokenProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Get<TestResponse>(EchoUrl);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        #endregion

        #region PostTests
        [Test]
        public void PostShouldReturnString()
        {
            var result = HttpHelper.Post(EchoUrl, string.Empty);
            var result2 = result.Message.Deserialize<TestResponse>();
            Assert.AreEqual("POST",result2.method);
        }
        [Test]
        public void PostShouldReturnStringWithBody()
        {
            var body = new { IsTestObject = true };
            var result = HttpHelper.Post(EchoUrl, body, string.Empty);
            Assert.IsTrue(result.Message.Contains("\"IsTestObject\": true"));
        }

        [Test]
        public void PostShouldReturnStringWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Post(EchoUrl);
            Assert.IsTrue(result.Message.Contains($"\"Authorization\": \"Bearer {id}\""));
        }
        [Test]
        public void PostShouldMapToObject()
        {
            var result = HttpHelper.Post<TestResponse>(EchoUrl, string.Empty);
            Assert.AreEqual("POST", result.Object.method);
        }
        [Test]
        public void PostShouldAddAuthHeader()
        {
            var id = Guid.NewGuid();
            var result = HttpHelper.Post<TestResponse>(EchoUrl, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PostShouldAddAuthHeaderFromTokenProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Post<TestResponse>(EchoUrl);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PostShouldMapBody()
        {
            var id = Guid.NewGuid();
            var body = new { IsTestObject = true };
            var result = HttpHelper.Post<TestResponse>(EchoUrl, body, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PostShouldMapBodyWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var body = new { IsTestObject = true };
            var result = HttpHelper.Post<TestResponse>(EchoUrl, body);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PostShouldReturnStringWithBodyAndTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var body = new { IsTestObject = true };
            var result = HttpHelper.Post(EchoUrl, body);
            Assert.IsTrue(result.Message.Contains("\"IsTestObject\": true"));
            Assert.IsTrue(result.Message.Contains($"\"Authorization\": \"Bearer {id}\""));
        }
        #endregion

        #region PutTests
        [Test]
        public void PutShouldReturnString()
        {
            var result = HttpHelper.Put(EchoUrl, string.Empty);
            Assert.IsTrue(result.Message.Contains("\"method\": \"PUT\""));
        }
        [Test]
        public void PutShouldReturnStringWithBody()
        {
            var body = new { IsTestObject = true };
            var result = HttpHelper.Put(EchoUrl, body, string.Empty);
            Assert.IsTrue(result.Message.Contains("\"IsTestObject\": true"));
        }

        [Test]
        public void PutShouldReturnStringWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Put(EchoUrl);
            Assert.IsTrue(result.Message.Contains($"\"Authorization\": \"Bearer {id}\""));
        }
        [Test]
        public void PutShouldMapToObject()
        {
            var result = HttpHelper.Put<TestResponse>(EchoUrl, string.Empty);
            Assert.AreEqual("PUT", result.Object.method);
        }
        [Test]
        public void PutShouldAddAuthHeader()
        {
            var id = Guid.NewGuid();
            var result = HttpHelper.Put<TestResponse>(EchoUrl, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PutShouldAddAuthHeaderFromTokenProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Put<TestResponse>(EchoUrl);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PutShouldMapBody()
        {
            var id = Guid.NewGuid();
            var body = new { IsTestObject = true };
            var result = HttpHelper.Put<TestResponse>(EchoUrl, body, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PutShouldMapBodyWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var body = new { IsTestObject = true };
            var result = HttpHelper.Put<TestResponse>(EchoUrl, body);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void PutShouldReturnStringWithBodyAndTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var body = new { IsTestObject = true };
            var result = HttpHelper.Put(EchoUrl, body);
            Assert.IsTrue(result.Message.Contains("\"IsTestObject\": true"));
            Assert.IsTrue(result.Message.Contains($"\"Authorization\": \"Bearer {id}\""));
        }
        #endregion

        #region DeleteTests
        [Test]
        public void DeleteShouldReturnString()
        {
            var result = HttpHelper.Delete(EchoUrl, string.Empty);
            Assert.IsTrue(result.Message.Contains("\"method\": \"DELETE\""));
        }
        [Test]
        public void DeleteShouldReturnStringWithBody()
        {
            var body = new { IsTestObject = true };
            var result = HttpHelper.Delete(EchoUrl, body, string.Empty);
            Assert.IsTrue(result.Message.Contains("\"IsTestObject\": true"));
        }

        [Test]
        public void DeleteShouldReturnStringWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Delete(EchoUrl);
            Assert.IsTrue(result.Message.Contains($"\"Authorization\": \"Bearer {id}\""));
        }
        [Test]
        public void DeleteShouldMapToObject()
        {
            var result = HttpHelper.Delete<TestResponse>(EchoUrl, string.Empty);
            Assert.AreEqual("DELETE", result.Object.method);
        }
        [Test]
        public void DeleteShouldAddAuthHeader()
        {
            var id = Guid.NewGuid();
            var result = HttpHelper.Delete<TestResponse>(EchoUrl, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void DeleteShouldAddAuthHeaderFromTokenProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var result = HttpHelper.Delete<TestResponse>(EchoUrl);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void DeleteShouldMapBody()
        {
            var id = Guid.NewGuid();
            var body = new { IsTestObject = true };
            var result = HttpHelper.Delete<TestResponse>(EchoUrl, body, id.ToString());
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void DeleteShouldMapBodyWithTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var body = new { IsTestObject = true };
            var result = HttpHelper.Delete<TestResponse>(EchoUrl, body);
            var authHeader = result.Object.headers.Authorization;
            Assert.AreEqual($"Bearer {id}", authHeader);
        }
        [Test]
        public void DeleteShouldReturnStringWithBodyAndTokenFromProperty()
        {
            var id = Guid.NewGuid();
            HttpHelper.SetToken(id.ToString());
            var body = new { IsTestObject = true };
            var result = HttpHelper.Delete(EchoUrl, body);
            Assert.IsTrue(result.Message.Contains("\"IsTestObject\": true"));
            Assert.IsTrue(result.Message.Contains($"\"Authorization\": \"Bearer {id}\""));
        }
        #endregion

    }





    public class Headers
    {
        public string Accept { get; set; }
        public string Authorization { get; set; }
        public string Host { get; set; }
        public string Requesttrackingid { get; set; }
        public string Requesttrackingsource { get; set; }

        [JsonProperty("X-Amzn-Trace-Id")]
        public string XAmznTraceId { get; set; }
    }

    public class TestResponse
    {
        public object args { get; set; }
        public string data { get; set; }
        public object files { get; set; }
        public object form { get; set; }
        public Headers headers { get; set; }
        public object json { get; set; }
        public string method { get; set; }
        public string origin { get; set; }
        public string url { get; set; }
    }

}
