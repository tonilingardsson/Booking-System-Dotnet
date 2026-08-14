using Booking_System.Api.Data;
using Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booking_System.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtsController : ControllerBase
    {
        private readonly BookingDbContext _context;

        public CourtsController(BookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Court>>> GetAll()
        {
            var courts = await _context.Courts
                .Include(c => c.Bookings)
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .ToListAsync();

            return Ok(courts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Court>> GetById(int id)
        {
            var court = await _context.Courts
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (court == null)
            {
                return NotFound();
            }

            return Ok(court);
        }

        [HttpPost]
        public async Task<ActionResult<Court>> Create(Court court)
        {
            if (string.IsNullOrWhiteSpace(court.CourtName))
            {
                return BadRequest("Court name is required.");
            }

            _context.Courts.Add(court);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = court.Id }, court);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Court updatedCourt)
        {
            if (id != updatedCourt.Id)
            {
                return BadRequest("Court id mismatch.");
            }

            var existingCourt = await _context.Courts.FindAsync(id);

            if (existingCourt == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(updatedCourt.CourtName))
            {
                return BadRequest("Court name is required.");
            }

            existingCourt.CourtName = updatedCourt.CourtName;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var court = await _context.Courts
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (court == null)
            {
                return NotFound();
            }

            if (court.Bookings != null && court.Bookings.Any())
            {
                return BadRequest("Cannot delete a court that has bookings.");
            }

            _context.Courts.Remove(court);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}