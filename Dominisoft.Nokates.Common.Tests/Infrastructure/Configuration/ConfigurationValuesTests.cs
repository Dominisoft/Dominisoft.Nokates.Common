using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.Configuration
{
    [TestFixture]
    public class ConfigurationValuesTests
    {
        private const string EchoUrl = "https://httpbin.org/anything";

        [Test]
        public void ConfigurationValueShouldLoadFromUrl()
        {
            ConfigurationValues.LoadConfigFromUrl("http://DevAppServer/Configuration/Hello");
            ConfigurationValues.TryGetValue(out var method, "Name");
            Assert.AreEqual("ale",method);
        }


    }
}
