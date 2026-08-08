using Booking_System.Api.Data;
using Booking_System.Api.Dtos;
using Booking_System.Api.Services;
using Booking_System.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Booking_System.Api.Services
{
    public class CustomerValidationException : Exception
    {
        public CustomerValidationException(string message) : base(message)
        { 
        // Write here the customized validation messages
        }
    }
    public class CustomerService : ICustomerService
    {
        private readonly BookingDbContext _context;

        public CustomerService(BookingDbContext context)
        {
            _context = context;
        }

        

        // -------------------------------------------------------------------
        // Basic CRUD
        // -------------------------------------------------------------------
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        // It has no validation at all, but you can add validation logic here if needed.
        // For example, you can check if the email address is valid or if the phone number is in the correct format.
        public async Task<Customer?> CreateCustomerAsync(Customer customer)
        {
            // Add validation logic here if needed
            if (await _context.Customers.AnyAsync(c => c.EmailAddress == customer.EmailAddress))
            {
                throw new CustomerValidationException("A customer with the same email address already exists.");
            }
            // You can add more validation logic here, such as checking if the phone number is in the correct format, etc.
            if (string.IsNullOrWhiteSpace(customer.FirstName) || string.IsNullOrWhiteSpace(customer.LastName))
            {
                throw new CustomerValidationException("First name and last name cannot be empty.");
            }
            // You can also add more complex validation logic, such as checking if the email address is in a valid format, etc.
            if (!IsValidEmail(customer.EmailAddress))
            {
                throw new CustomerValidationException("Invalid email address format.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        private bool IsValidEmail(string emailAddress)
        {
            /*            throw new NotImplementedException("Email validation logic not implemented.");
             *            
             */
            return !string.IsNullOrWhiteSpace(emailAddress) && emailAddress.Contains("@");

        }

        // It takes two parameters but the current tests only check for the first parameter,
        // so you can add validation logic here if needed.
        public async Task<Customer?> UpdateCustomerAsync(int id, Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);

            if (existingCustomer is null)
            {
                return null;
            }

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.EmailAddress = customer.EmailAddress;
            existingCustomer.PhoneNumber = customer.PhoneNumber;

            await _context.SaveChangesAsync();
            return existingCustomer;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer is null)
            { 
                return false; 
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
