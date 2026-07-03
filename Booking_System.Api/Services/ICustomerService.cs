using Booking_System.Models;
using Booking_System.Api.Dtos;

namespace Booking_System_Dotnet.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer?> GetCustomerByIdAsync(int id);
    }
}
