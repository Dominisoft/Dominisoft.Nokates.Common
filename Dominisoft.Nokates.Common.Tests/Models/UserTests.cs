using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void UserShouldStoreValues()
        {
            #region Arrange

            const int id = 12345;
            const string firstName = "firstName";
            const string lastName = "lastName";
            const string username = "username";
            const string email = "email@example.com";
            const string roles = "[]";
            const string additionalEndpointPermissions = "[]";
            const string phoneNumber = "123456789";
            const bool isActive = true;

            #endregion

            #region Act

            var sut = new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email,
                Roles = roles,
                AdditionalEndpointPermissions = additionalEndpointPermissions,
                PhoneNumber = phoneNumber,
                IsActive = isActive

            };

            #endregion

            #region Assert

            Assert.AreEqual(id, sut.Id);
            Assert.AreEqual(firstName, sut.FirstName);
            Assert.AreEqual(lastName, sut.LastName);
            Assert.AreEqual(username, sut.Username);
            Assert.AreEqual(email, sut.Email);
            Assert.AreEqual(roles, sut.Roles);
            Assert.AreEqual(additionalEndpointPermissions, sut.AdditionalEndpointPermissions);
            Assert.AreEqual(phoneNumber, sut.PhoneNumber);
            Assert.AreEqual(isActive, sut.IsActive);

            #endregion
        }
    }
}
