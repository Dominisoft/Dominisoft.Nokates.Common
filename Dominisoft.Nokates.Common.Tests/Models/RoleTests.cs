using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class RoleTests
    {
        [Test]
        public void RoleShouldStoreValues()
        {
            #region Arrange

            const int id = 12345;
            const string name = "Admin";
            const string description = "sys admin";
            const string allowedEndpoints = "[]";
            #endregion;


            #region Act

            var sut = new Role
            {
                Id = id,
                Name = name,
                Description = description,
                AllowedEndpoints = allowedEndpoints
            };

            #endregion

            #region Assert

            Assert.AreEqual(id, sut.Id, "Entity ID did not match passed value");
            Assert.AreEqual(name, sut.Name);
            Assert.AreEqual(description, sut.Description);
            Assert.AreEqual(allowedEndpoints, sut.AllowedEndpoints);

            #endregion
        }
    }
}