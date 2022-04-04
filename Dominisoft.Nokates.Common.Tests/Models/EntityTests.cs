using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void EntityShouldStoreId()
        {
            #region Arrange

            const int id = 12345;

            #endregion

            #region Act

            var sut = new Entity {Id = id};

            #endregion

            #region Assert
            
            Assert.AreEqual(id,sut.Id,"Entity ID did not match passed value");

            #endregion
        }
    }
}