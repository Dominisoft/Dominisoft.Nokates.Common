using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.CustomExceptions;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.CustomExceptions
{
    [TestFixture]
    public class RequestExceptionTests
    {
        [Test]
        public void BaseRequestExceptionShouldContainCorrectMessageAndResponseCode()
        {
            #region Arrange

            const string msg = "Testing";
            const int code = 123;
            #endregion

            #region Act

            var exception = new RequestException(code,msg);

            #endregion

            #region Assert

            Assert.AreEqual(code, exception.StatusCode,"Incorrect Response Code");
            Assert.AreEqual(msg, exception.Message,"Incorrect Response Code");

            #endregion

        }
    }
}
