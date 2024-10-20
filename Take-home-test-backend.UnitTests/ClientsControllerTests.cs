using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Take_home_test_backend.Controllers;
using Take_home_test_backend.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Take_home_test_backend.Tests.Controllers
{
    [TestClass]
    public class ClientsControllerTests
    {
        private Mock<DataService> _mockDataService;
        private ClientsController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockDataService = new Mock<DataService>();
            _controller = new ClientsController(_mockDataService.Object);
        }

        [TestMethod]
        public void GetClientInfo_ClientExists_ReturnsOkResult()
        {
            // Arrange
            var clientId = "123";
            var client = new Client { ClientId = clientId, Name = "John Doe", Email = "john@example.com", Address = "123 Main St" };
            _mockDataService.Setup(ds => ds.Clients).Returns(new List<Client> { client });

            // Act
            var result = _controller.GetClientInfo(clientId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(client, okResult.Value);
        }

        [TestMethod]
        public void GetClientInfo_ClientDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var clientId = "123";
            _mockDataService.Setup(ds => ds.Clients).Returns(new List<Client>());

            // Act
            var result = _controller.GetClientInfo(clientId);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetClients_NoNameProvided_ReturnsAllClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { ClientId = "123", Name = "John Doe", Email = "john@example.com", Address = "123 Main St" },
                new Client { ClientId = "456", Name = "Jane Doe", Email = "jane@example.com", Address = "456 Elm St" }
            };
            _mockDataService.Setup(ds => ds.Clients).Returns(clients);

            // Act
            var result = _controller.GetClients(null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(clients, okResult.Value);
        }

        [TestMethod]
        public void GetClients_NameProvided_ReturnsMatchingClients()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { ClientId = "123", Name = "John Doe", Email = "john@example.com", Address = "123 Main St" },
                new Client { ClientId = "456", Name = "Jane Doe", Email = "jane@example.com", Address = "456 Elm St" }
            };
            _mockDataService.Setup(ds => ds.Clients).Returns(clients);

            // Act
            var result = _controller.GetClients("John Doe");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<Client>));
            var clientsResult = okResult.Value as IEnumerable<Client>;
            foreach (var client in clientsResult)
            {
                Assert.AreEqual("John Doe", client.Name);
            }
        }

        [TestMethod]
        public void GetClients_NameProvided_NoMatchingClients_ReturnsNotFoundResult()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { ClientId = "123", Name = "John Doe", Email = "john@example.com", Address = "123 Main St" },
                new Client { ClientId = "456", Name = "Jane Doe", Email = "jane@example.com", Address = "456 Elm St" }
            };
            _mockDataService.Setup(ds => ds.Clients).Returns(clients);

            // Act
            var result = _controller.GetClients("Nonexistent Name");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
    }
}
