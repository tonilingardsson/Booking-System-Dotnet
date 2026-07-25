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
        public async Task GetCustomerByIdAsync_ExistingId_ReturnsCustomer()
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
            var result = await _service.GetCustomerByIdAsync(customer.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(customer.Id, result.Id);
            Assert.AreEqual(customer.FirstName, result.FirstName);
            Assert.AreEqual(customer.LastName, result.LastName);
            Assert.AreEqual(customer.EmailAddress, result.EmailAddress);
            Assert.AreEqual(customer.PhoneNumber, result.PhoneNumber);
        }

    }
}
