using System;
using Dominisoft.Nokates.Common.Models;
using Microsoft.VisualBasic;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class RequestMetricTests
    {
        [Test]
        public void RequestMetricShouldStoreValues()
        {
            #region Arrange

            const int id = 12345;
            const string requestType = "GET";
            const string requestJson = "{\"IsJson\":\"true\'}";
            const string serviceName = "Test";
            const string requestPath = "./Path/to/my/resource";
            const string responseJson = "{\"IsResponse\":\"true\'}";
            const string endpointDesignation = "Test:Controller.Method";
            const int responseCode = 200;
            DateTime requestStart = DateTime.Now;
            const long responseTime = 100;

            #endregion

            #region Act

            var sut = new RequestMetric
            {
                Id = id,
                RequestType = requestType,
                RequestJson = requestJson,
                ServiceName = serviceName,
                RequestPath = requestPath,
                ResponseJson = responseJson,
                EndpointDesignation = endpointDesignation,
                ResponseCode = responseCode,
                RequestStart = requestStart,
                ResponseTime = responseTime

            };

            #endregion

            #region Assert

            Assert.AreEqual(id, sut.Id);
            Assert.AreEqual(requestType, sut.RequestType);
            Assert.AreEqual(serviceName, sut.ServiceName);
            Assert.AreEqual(requestPath, sut.RequestPath);
            Assert.AreEqual(requestJson, sut.RequestJson);
            Assert.AreEqual(responseJson, sut.ResponseJson);
            Assert.AreEqual(endpointDesignation, sut.EndpointDesignation);
            Assert.AreEqual(responseCode, sut.ResponseCode);
            Assert.AreEqual(requestStart, sut.RequestStart);
            Assert.AreEqual(responseTime, sut.ResponseTime);

            #endregion
        }
    }
}