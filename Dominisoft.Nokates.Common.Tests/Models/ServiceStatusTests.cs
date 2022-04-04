using System;
using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class ServiceStatusTests
    {
        [Test]
        public void EntityShouldStoreId()
        {
            #region Arrange

            const string name = "test";
            var startTime = DateTime.Now;
            const bool isOnline = true;
            const string uri = "localhost/app/path";

            const string version = "V1.0.0";
            var buildDate = DateTime.Now.AddDays(-1);
            var deploymentDate = DateTime.Now.AddHours(-1);
            const string branch = "master";
            const string environment = "test";
            const string lastCommitId = "asdasdasdasdasdasd";
            var versionDetails = new VersionDetails
            {
                Version = version,
                BuildDate = buildDate,
                DeploymentDate = deploymentDate,
                Branch = branch,
                Environment = environment,
                LastCommitId = lastCommitId

            };

            #endregion

            #region Act

            var sut = new ServiceStatus
            {
                Name = name,
                StartTime = startTime,
                IsOnline = isOnline,
                Uri = uri,
                Version = versionDetails
            };

            #endregion

            #region Assert

            Assert.AreEqual(startTime, sut.StartTime);
            Assert.AreEqual(name, sut.Name);
            Assert.AreEqual(isOnline, sut.IsOnline);
            Assert.AreEqual(uri, sut.Uri);
            Assert.AreEqual(versionDetails, sut.Version);

            #endregion
        }
    }
}