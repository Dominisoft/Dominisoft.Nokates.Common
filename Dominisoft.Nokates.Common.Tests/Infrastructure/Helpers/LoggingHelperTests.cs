using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using Dominisoft.Nokates.Common.Infrastructure.Helpers;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.Helpers
{
    [TestFixture]
    public class LoggingHelperTests
    {
        [Test]
        public void LogShouldAddMessageToStatusValues()
        {
            #region Arrange

            const string message = "Test message";

            #endregion

            #region Act

            LoggingHelper.LogMessage(message);

            #endregion

            #region Assert

            Assert.IsTrue(StatusValues.EventLog.Any(e => e.Message == message));

            #endregion
        }
    }
}
