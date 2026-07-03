using Booking_System.Models;
using Booking_System.Api.Dtos;

namespace Booking_System.Api.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        // Task<Customer?> GetCustomerByIdAsync(int id);
    }
}
