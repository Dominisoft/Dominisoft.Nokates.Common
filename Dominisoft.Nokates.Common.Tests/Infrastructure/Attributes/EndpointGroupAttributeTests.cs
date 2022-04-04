using Dominisoft.Nokates.Common.Infrastructure.Attributes;
using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Infrastructure.Attributes
{
    [TestFixture]
    public class EndpointGroupAttributeTests
    {
        [Test]
        public void AttributeShouldStoreListOfGroups()
        {
            #region Arrange

            var endpointGroups = new[]
            {
                "Test1", "Test2"
            };

            #endregion

            #region Act

            var sut = new EndpointGroup(endpointGroups[0], endpointGroups[1]);

            #endregion

            #region Assert

            Assert.IsTrue(sut.Groups.Count==2);

            #endregion
        }
    }
}