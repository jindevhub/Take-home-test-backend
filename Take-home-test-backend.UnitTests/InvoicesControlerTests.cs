using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Take_home_test_backend.Controllers;
using Take_home_test_backend.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Take_home_test_backend.Tests.Controllers
{
    [TestClass]
    public class InvoicesControllerTests
    {
        private Mock<DataService> _mockDataService;
        private InvoicesController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockDataService = new Mock<DataService>();
            _controller = new InvoicesController(_mockDataService.Object);
        }

        [TestMethod]
        public void GetInvoiceInfo_InvoiceExists_ReturnsOkResult()
        {
            // Arrange
            var invoiceNumber = "INV-001";
            var invoice = new Invoice { InvoiceNumber = invoiceNumber, ClientId = "123", DueDate = new DateTime(2022, 12, 31), Status = "Paid" };
            _mockDataService.Setup(ds => ds.Invoices).Returns(new List<Invoice> { invoice });

            // Act
            var result = _controller.GetInvoiceByInvoiceNumber(invoiceNumber);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(invoice, okResult.Value);
        }

        [TestMethod]
        public void GetInvoiceInfo_InvoiceDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var invoiceNumber = "INV-001";
            _mockDataService.Setup(ds => ds.Invoices).Returns(new List<Invoice>());

            // Act
            var result = _controller.GetInvoiceByInvoiceNumber(invoiceNumber);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetInvoices_NoStatusProvided_ReturnsAllInvoices()
        {
            // Arrange
            var invoices = new List<Invoice>
            {
                new Invoice { InvoiceNumber = "INV-001", ClientId = "123", DueDate = new DateTime(2022, 12, 31), Status = "Paid" },
                new Invoice { InvoiceNumber = "INV-002", ClientId = "456", DueDate = new DateTime(2022, 12, 31), Status = "Pending" }
            };
            _mockDataService.Setup(ds => ds.Invoices).Returns(invoices);

            // Act
            var result = _controller.GetInvoices(null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(invoices, okResult.Value);
        }

        [TestMethod]
        public void GetInvoices_StatusProvided_ReturnsMatchingInvoices()
        {
            // Arrange
            var invoices = new List<Invoice>
            {
                new Invoice { InvoiceNumber = "INV-001", ClientId = "123", DueDate = new DateTime(2022, 12, 31), Status = "Paid" },
                new Invoice { InvoiceNumber = "INV-002", ClientId = "456", DueDate = new DateTime(2022, 12, 31), Status = "Pending" }
            };
            var client = new Client { ClientId = "123", Name = "John Doe", Email = "john@example.com", Address = "123 Main St" };
            _mockDataService.Setup(ds => ds.Clients).Returns(new List<Client> { client });
            _mockDataService.Setup(ds => ds.Invoices).Returns(invoices);

            // Act
            var result = _controller.GetInvoices("John Doe");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<Invoice>));
            var invoicesResult = okResult.Value as IEnumerable<Invoice>;
            foreach (var invoice in invoicesResult)
            {
                Assert.AreEqual("Paid", invoice.Status);
            }
        }

        [TestMethod]
        public void GetInvoices_StatusProvided_NoMatchingInvoices_ReturnsNotFoundResult()
        {
            // Arrange
            var invoices = new List<Invoice>
            {
                new Invoice { InvoiceNumber = "INV-001", ClientId = "123", DueDate = new DateTime(2022, 12, 31), Status = "Paid" },
                new Invoice { InvoiceNumber = "INV-002", ClientId = "456", DueDate = new DateTime(2022, 12, 31), Status = "Pending" }
            };
            var client = new Client { ClientId = "123", Name = "John Doe", Email = "john@example.com", Address = "123 Main St" };
            _mockDataService.Setup(ds => ds.Clients).Returns(new List<Client> { client });
            _mockDataService.Setup(ds => ds.Invoices).Returns(invoices);

            // Act
            var result = _controller.GetInvoices("Nonexistent Name");

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
    }
}
