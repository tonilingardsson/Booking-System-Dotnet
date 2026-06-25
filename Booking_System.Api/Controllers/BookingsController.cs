using Booking_System.Api.Services;
using Booking_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Booking_System.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/bookings/8
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);

            if (booking is null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // POST: api/bookings
        [HttpPost]
        // [FromBody] is an attribute applied to the parameter:
        // [FromBody] tells ASP.NET Core to read the JSON request body and
        // bind it to the method parameter, which is common for POST requests.
        // ASP.NET Core additionally sees: “this parameter has the [FromBody] attribute,
        // so I’ll bind it from the HTTP request body.”

        public async Task<ActionResult<Booking>> Create([FromBody] Booking booking) 
        {
            try
            {
                var createdBooking = await _bookingService.CreateBookingAsync(booking);

            return CreatedAtAction(
                nameof(GetBookingById),
                new { id = createdBooking.Id },
                createdBooking);
            }
            catch (BookingValidationException ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Booking>> Update(int id, [FromBody] Booking booking)
        {
            if (id != booking.Id) return BadRequest(new { message = "Id mismatch." });

            try
            {
                var updated = await _bookingService.UpdateBookingAsync(booking);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (BookingValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}