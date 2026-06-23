using Booking_System.Api.Data;
using Booking_System.Api.Dtos;
using Booking_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking_System.Api.Services
{
    // Custom exception usend when business rules are violated
    // Controllers can catch this and return a 400 Bad Request with the message
    public class BookingValidationException : Exception
    {
        public BookingValidationException(string message) : base(message)
            { }
    }

    // Concrete implementation of IBookingService
    // All booking-related business logic and validation lives here,
    // not in the controllers
    public class BookingService : IBookingService
    {
        private readonly BookingDbContext _context;
        // Constructor injection of the DbContext.
        // ASP.NET Core will create BookingService instances and
        // pass a BookingDbContext automatically (after we register it in Program.cs)
        public BookingService(BookingDbContext context)
        {
            _context = context;
        }

        // -------------------------------------------------------------------
        // Basic CRUD
        // -------------------------------------------------------------------
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Court)
                .ToListAsync();
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            // Find the booking with given ID, or null if not found
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Court)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            // Normalize and validate booking data before saving
            NormalizeBooking(booking);
            await ValidateBookingAsync(booking, isUpdate: false);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking?> UpdateBookingAsync(Booking booking)
        {
            // Check if the booking exists in the database
            var existingBooking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == booking.Id);

            if (existingBooking is null)
            {
                return null;
            }

            // Update allowed fields on the existing entity
            existingBooking.CourtId = booking.CourtId;
            existingBooking.CustomerId = booking.CustomerId;
            existingBooking.StartTime = booking.StartTime;

            NormalizeBooking(existingBooking);
            await ValidateBookingAsync(existingBooking, isUpdate: true);

            await _context.SaveChangesAsync();
            return existingBooking;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var existingBooking = await _context.Bookings.FindAsync(id);

            if (existingBooking is null)
            {
                return false;
            }

            _context.Bookings.Remove(existingBooking);
            await _context.SaveChangesAsync();

            return true;
        }

        // ---------------------------------------------------------
        // Queries for user stories
        // ---------------------------------------------------------

        public async Task<IEnumerable<Booking>> GetBookingsForDayAsync(DateOnly date)
        {
            // Convert DateOnly to a DateTime range covering the full day
            var startOfDay = date.ToDateTime(TimeOnly.MinValue);
            var endOfDay = date.ToDateTime(TimeOnly.MaxValue);

            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Court)
                .Where(b => b.StartTime >= startOfDay && b.StartTime <= endOfDay)
                .OrderBy(b => b.StartTime)
                .ToListAsync(); // I remember that the teacher does not like when we use too much .Include()
        }

        public async Task<IEnumerable<AvailableSlotDto>> GetAvailabilityAsync(DateOnly startDate, DateOnly endDate)
        {
            // This method will:
            // 1. Generate all possible 1-hour slots for each court between the two dates.
            // 2. Load all bookings that fall in that range.
            // 3. Filter out the slots that are already booked.

            var slots = new List<AvailableSlotDto>();

            // TODO: Implement availability calculation
            // Hint: 
            // - loop dates from startDate to endDate (inclusive)
            // - for each date, for each court, loop hours 7 to 21 and 
            //   create candidate slots; exclude hours with existing bookings.

            return slots;
        }

        public async Task<IEnumerable<CourtStatisticsDto>> GetStatisticsAsync(DateOnly startDate, DateOnly endDate)
        {
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.ToDateTime(TimeOnly.MaxValue);

            var query = await _context.Bookings
                .Include(b => b.Court)
                .Where(b => b.StartTime >= start && b.StartTime <= end)
                .GroupBy(b => new { b.CourtId, b.Court!.CourtName})
                .Select(g => new CourtStatisticsDto
                {
                    CourtId = g.Key.CourtId,
                    CourtName = g.Key.CourtName,
                    BookingCount = g.Count()
                })
                .ToListAsync();

            return query;
        }

        // ---------------------------------------------------------------
        // Validation helpers
        // ---------------------------------------------------------------

        // Ensure the booking data is in a consistent state
        // This is where you could round times, enforce UTC/local, etc.

        private void NormalizeBooking(Booking booking)
        {
            // If you want, you can enforce seconds and milliseconds to zero
            booking.StartTime = new DateTime(
                booking.StartTime.Year,
                booking.StartTime.Month,
                booking.StartTime.Day,
                booking.StartTime.Hour,
                booking.StartTime.Minute,
                0,
                DateTimeKind.Local);
        }

        // Central place for all booking validation rules
        // Throws BookinValidationException when any rule is violated
        private async Task ValidateBookingAsync(Booking booking, bool isUpdate)
        {
            // Rule 1: Start time must be on a whole hour (minutes = 0)
            if (booking.StartTime.Minute != 0)
            {
                throw new BookingValidationException("Bookings must start on whole hours (e.g. 13:00).");
            }

            var hour = booking.StartTime.Hour;

            // Rule 2: Opening hours are between 7 and 22
            // With 1 hour booking, valid start hours are 7 to 21, inclusive.
            if (hour < 7 || hour > 21)
            {
                throw new BookingValidationException("Bookings are only allowed between 7:00 and 22:00.");
            }

            // Rule 3: Customer and court must exist.
            var customerExists = await _context.Customers.AnyAsync(c => c.Id == booking.CustomerId);
            if (!customerExists)
            {
                throw new BookingValidationException("Customer does not exist.");
            }

            var courtExists = await _context.Courts.AnyAsync(c => c.Id == booking.CourtId);
            if (!courtExists)
            {
                throw new BookingValidationException("Court does not exist.");
            }

            var start = booking.StartTime;

            var overlappingQuery = _context.Bookings
                .Where(b => b.CourtId == booking.CourtId && b.StartTime == start);

            if (isUpdate)
            {
                overlappingQuery = overlappingQuery.Where(b => b.Id != booking.Id);
            }

            var overlapExists = await overlappingQuery.AnyAsync();
            if (overlapExists)
            {
                throw new BookingValidationException("This court is already booked at that time.");
            }
        }
    } 
}