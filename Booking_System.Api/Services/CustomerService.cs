using Booking_System.Api.Data;
using Booking_System.Api.Dtos;
using Booking_System.Api.Services;
using Booking_System.Models;
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
        public async Task<Customer?> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
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
