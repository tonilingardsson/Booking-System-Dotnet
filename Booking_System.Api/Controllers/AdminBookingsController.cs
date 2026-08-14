using Booking_System.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_System.Api.Controllers
{
    [ApiController]
    [Route("api/admin/bookings")]
    public class AdminBookingsController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public AdminBookingsController(BookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("day")]
        public async Task<IActionResult> GetBookingsForDay([FromQuery] DateTime date)
        {
            var dayStart = date.Date;
            var dayEnd = dayStart.AddDays(1);

            var bookings = await _context.Bookings
                .Where(b => b.StartTime >= dayStart && b.StartTime < dayEnd)
                .Include(b => b.Customer)
                .Include(b => b.Court)
                .OrderBy(b => b.StartTime)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("interval")]
        public async Task<IActionResult> GetBookingsInterval(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            if (endDate < startDate)
            {
                return BadRequest("End date must be greater than or equal to start date.");
            }

            var rangeStart = startDate.Date;
            var rangeEndExclusive = endDate.Date.AddDays(1);

            var bookingsPerCourt = await _context.Bookings
                .Where(b => b.StartTime >= rangeStart && b.StartTime < rangeEndExclusive)
                .GroupBy(b => new
                {
                    b.CourtId,
                    CourtName = b.Court != null ? b.Court.CourtName : "Unknown Court"
                })
                .Select(g => new
                {
                    CourtId = g.Key.CourtId,
                    CourtName = g.Key.CourtName,
                    BookingCount = g.Count()
                })
                .OrderBy(x => x.CourtName)
                .ToListAsync();

            var totalBookingCount = bookingsPerCourt.Sum(x => x.BookingCount);

            var result = new
            {
                StartDate = rangeStart,
                EndDate = endDate.Date,
                TotalBookingCount = totalBookingCount,
                BookingsPerCourt = bookingsPerCourt
            };

            return Ok(result);
        }
    }
}