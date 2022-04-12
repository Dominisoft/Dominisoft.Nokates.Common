using System;
using System.Collections.Generic;
using System.Text;
using Dominisoft.Nokates.Common.Infrastructure.Client;
using Dominisoft.Nokates.Common.Infrastructure.Configuration;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.Client
{
    [TestFixture]
    public class AuthenticationClientTests
    {
        private const string UserName = "ServiceLogin";
        private const string Password = "asdf";
        private const string authUrl = "http://DevAppServer/Identity/Authentication";
        [Test]
        public void AuthShouldReturnValidToken()
        {
            var authClient = new AuthenticationClient(authUrl);

            var token = authClient.GetToken(UserName, Password);

            Assert.IsTrue(token?.Split('.').Length == 3);

        }
  
    }
}
