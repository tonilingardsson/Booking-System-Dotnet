using Booking_System.Models;
using Booking_System.Api.Dtos;

namespace Booking_System.Api.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();

        Task<Customer?> GetCustomerByIdAsync(int id);
        // We don't need the id because it is created with the customer object
        Task<Customer?> CreateCustomerAsync(Customer customer);
        Task<Customer?> UpdateCustomerAsync(int id, Customer customer);
        // We don't need customer object because it is going to be deleted using the id
        Task<bool> DeleteCustomerAsync(int id);
    }
}
