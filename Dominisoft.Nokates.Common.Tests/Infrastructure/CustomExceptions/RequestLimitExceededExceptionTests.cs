using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.CustomExceptions;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.CustomExceptions
{
    [TestFixture]
    public class RequestLimitExceededExceptionTests
    {
        [Test]
        public void ExceptionShouldContainCorrectMessageAndResponseCode()
        {
            #region Arrange

            const string msg = "Testing";

            #endregion

            #region Act

            var exception = new RequestLimitExceededException(msg);

            #endregion

            #region Assert

            Assert.AreEqual(429, exception.StatusCode,"Incorrect Response Code");
            Assert.AreEqual(msg, exception.Message,"Incorrect Response Code");

            #endregion

        }
    }
}
