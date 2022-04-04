using System;
using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class VersionDetailsTests
    {
        [Test]
        public void EndpointDescriptionShouldStoreValues()
        {
            #region Arrange

            const string version = "V1.0.0";
            var buildDate = DateTime.Now.AddDays(-1);
            var deploymentDate = DateTime.Now.AddHours(-1);
            const string branch = "master";
            const string environment = "test";
            const string lastCommitId = "asdasdasdasdasdasd";
            #endregion

            #region Act

            var sut = new VersionDetails
            {
                Version = version,
                BuildDate = buildDate,
                DeploymentDate = deploymentDate,
                Branch = branch,
                Environment = environment,
                LastCommitId = lastCommitId

            };

            #endregion

            #region Assert

            Assert.AreEqual(version, sut.Version);
            Assert.AreEqual(branch, sut.Branch);
            Assert.AreEqual(buildDate, sut.BuildDate);
            Assert.AreEqual(deploymentDate, sut.DeploymentDate);
            Assert.AreEqual(environment, sut.Environment);
            Assert.AreEqual(lastCommitId, sut.LastCommitId);

            #endregion
        }

    }
}