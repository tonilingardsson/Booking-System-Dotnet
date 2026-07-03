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
    }
}
