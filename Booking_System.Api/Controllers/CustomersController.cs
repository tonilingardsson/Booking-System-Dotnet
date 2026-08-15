using Booking_System.Api.Services;
using Booking_System.Dtos;
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

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(CustomerDto dto)
        {
            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                PhoneNumber = dto.PhoneNumber
            };


            try
            {
            var createdCustomer = await _customerService.CreateCustomerAsync(customer);

            if (createdCustomer is null) {
                return BadRequest(new { message = "Customer could not be created." });
            } 
            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (CustomerValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, CustomerDto dto)
        {
            var existingCustomer = await _customerService.GetCustomerByIdAsync(id);
            
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.FirstName = dto.FirstName;
            existingCustomer.LastName = dto.LastName;
            existingCustomer.EmailAddress = dto.EmailAddress;
            existingCustomer.PhoneNumber = dto.PhoneNumber;

            try
            {
            var updatedCustomer = await _customerService.UpdateCustomerAsync(id,existingCustomer);

            return Ok(updatedCustomer);
            }
            catch (CustomerValidationException ex)
            {
                return BadRequest(ex.Message);
            }
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