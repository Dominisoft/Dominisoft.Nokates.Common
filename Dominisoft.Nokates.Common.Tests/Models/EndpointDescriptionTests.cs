using Dominisoft.Nokates.Common.Models;
using NUnit.Framework;

namespace Dominisoft.Nokates.Common.Tests.Models
{
    [TestFixture]
    public class EndpointDescriptionTests
    {
        [Test]
        public void EndpointDescriptionShouldStoreValues()
        {
            #region Arrange

            const string method = "GET";
            const string route = "./value";
            const string action = "value";
            const string controllerMethod = "GetValue";
            const string controller = "ValueController";
            const string appName = "myTestApp";
            const bool hasNoAuthAttribute = true;

            #endregion

            #region Act

            var sut = new EndpointDescription
            {
                Method = method,
                Route = route,
                Action = action,
                ControllerMethod = controllerMethod,
                AppName = appName,
                HasNoAuthAttribute = hasNoAuthAttribute,
                Controller = controller
            };

            #endregion

            #region Assert

            Assert.AreEqual(method, sut.Method);
            Assert.AreEqual(route, sut.Route);
            Assert.AreEqual(action, sut.Action);
            Assert.AreEqual(controller, sut.Controller);
            Assert.AreEqual(controllerMethod, sut.ControllerMethod);
            Assert.AreEqual(hasNoAuthAttribute, sut.HasNoAuthAttribute);
            Assert.AreEqual(appName, sut.AppName);

            #endregion
        }
    }
}