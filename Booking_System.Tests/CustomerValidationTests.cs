using Booking_System.Api.Data;
using Booking_System.Api.Services;
using Booking_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Booking_System.Tests
{
    [TestClass]
    public class CustomerValidationTests
    {
        private BookingDbContext _context = null!;
        private CustomerService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new BookingDbContext(options);

            _context.Customers.Add(new Customer
            {
                Id = 1,
                FirstName = "Antonio",
                LastName = "Luna",
                EmailAddress = "antonio@luna.com",
                PhoneNumber = "0729291305"
            });

            _context.SaveChanges();
            _service = new CustomerService(_context);
        }

        [TestMethod]

        public async Task GetAllCustomersAsync_ReturnsCustomers()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 2,
                FirstName = "Toni",
                LastName = "Luna",
                EmailAddress = "antonio@luna.com",
                PhoneNumber = "0701234567"
            };

            // Act & Assert

            var exception = await Assert.ThrowsAsync<CustomerValidationException>(() => _service.CreateCustomerAsync(customer));
            // It should return the first customer in the database, which is Antonio Luna
            // and the second customer should not be added due to the duplicate email address
            Assert.AreEqual("A customer with the same email address already exists.", exception.Message);
        }

        [TestMethod]
        public async Task GetCustomerByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            int nonExistingId = 999;
            // Act
            var result = await _service.GetCustomerByIdAsync(nonExistingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task CreateCustomerAsync_ValidCustomer_CreatesCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 2,
                FirstName = "Toni",
                LastName = "Luna",
                EmailAddress = "toni@luna.com",
                PhoneNumber = "0701234567"
            };

            // Act
            var result = await _service.CreateCustomerAsync(customer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Id, result.Id);
            Assert.AreEqual(customer.FirstName, result.FirstName);
            Assert.AreEqual(customer.LastName, result.LastName);
            Assert.AreEqual(customer.EmailAddress, result.EmailAddress);
            Assert.AreEqual(customer.PhoneNumber, result.PhoneNumber);
        }

        [TestMethod]
        public async Task UpdateCustomerAsync_ExistingId_UpdatesCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Antonio",
                LastName = "Luna",
                EmailAddress = "antonio@luna.com",
                PhoneNumber = "0729291305"
            };

            // Act
            customer.FirstName = "UpdatedFirstName";
            customer.LastName = "UpdatedLastName";
            customer.EmailAddress = "toni@luna.com";
            customer.PhoneNumber = "0701234567";

            var result = await _service.UpdateCustomerAsync(customer.Id, customer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.FirstName, result.FirstName);
            Assert.AreEqual(customer.LastName, result.LastName);
            Assert.AreEqual(customer.EmailAddress, result.EmailAddress);
            Assert.AreEqual(customer.PhoneNumber, result.PhoneNumber);
        }

        [TestMethod]
        public async Task UpdateCustomerAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 999,
                FirstName = "NonExisting",
                LastName = "Customer",
                EmailAddress = "antonio@luna.com",
                PhoneNumber = "0729291305"
            };

            // Act
            var result = await _service.UpdateCustomerAsync(customer.Id, customer);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteCustomerAsync_ExistingId_DeletesCustomer()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                FirstName = "Antonio",
                LastName = "Luna",
                EmailAddress = "antonio@luna.com",
                PhoneNumber = "0729291305"
            };

            // Act
            await _service.DeleteCustomerAsync(customer.Id);

            // Assert
            var result = await _service.GetCustomerByIdAsync(customer.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DeleteCustomerAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var result = await _service.DeleteCustomerAsync(nonExistingId);

            // Assert
            Assert.IsFalse(result);
        }

        // TODO: inside this test class does nothing unless you call it from a test method.
        // Consider removing it or adding a test method that calls it.
        // Validation belongs in the service i you want to test it,
        // you should call the service method that does the validation.
        private void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
                throw new CustomerValidationException("First name is required.");

            if (string.IsNullOrWhiteSpace(customer.LastName))
                throw new CustomerValidationException("Last name is required.");

            if (string.IsNullOrWhiteSpace(customer.EmailAddress))
                throw new CustomerValidationException("Email address is required.");
        }
    }
}