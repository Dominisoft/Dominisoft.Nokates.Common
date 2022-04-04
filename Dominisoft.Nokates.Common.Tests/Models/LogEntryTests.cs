using System;
using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class LogEntryTests
    {
        [Test]
        public void LogEntryShouldStoreValues()
        {
            #region Arrange

            const int id = 12345;
            const string message = "Unit test has been run";
            const string source = "Unit test has been run";
            var date = DateTime.Now.AddHours(-1);

            #endregion

            #region Act

            var sut = new LogEntry
            {
                Id = id,
                Message = message,
                Source = source,
                Date = date
            };

            #endregion

            #region Assert

            Assert.AreEqual(id, sut.Id, "Entity ID did not match passed value");
            Assert.AreEqual(message, sut.Message);
            Assert.AreEqual(source, sut.Source);
            Assert.AreEqual(date, sut.Date);

            #endregion
        }
    }
}