using Booking_System.Api.Services;
using Booking_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking_System.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);

        }
        // GET: api/customers
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer is null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // Post a new customer
        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            var createdCustomer = await _customerService.CreateCustomerAsync(customer);

            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> UpdateCustomer(int id, Customer customer)
        {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customer);

            if (updatedCustomer is null)
            {
                return NotFound();
            }

            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            bool deleted = await _customerService.DeleteCustomerAsync(id);

            if (!deleted)
            { 
                return NotFound(); 
            }
            return NoContent();
        }
    }
}