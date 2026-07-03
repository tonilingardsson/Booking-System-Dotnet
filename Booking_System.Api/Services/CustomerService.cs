using Booking_System.Api.Data;
using Booking_System.Api.Dtos;
using Booking_System.Api.Services;
using Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_System_Dotnet.Services
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
        private readonly CustomerDbContext _context;

        public CustomerService(CustomerDbContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------------------
        // Basic CRUD
        // -------------------------------------------------------------------
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .ToListAsync();
        }
    }
}
