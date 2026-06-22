using Booking_System.Models;

namespace Booking_System.Api.Services
{
    // This interface defines the contract for booking operations 
    // The controller will only know about this interface, not the concrete class
    // That makes it easier to test and swap implementations later
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        
        Task<Booking?> GetBookingByIdAsync(int id);

        // Creates a new booking after validateing business rules
        // Throws BookingValidationException if any rule is broken
        Task<Booking> CreateBookingAsync(Booking booking);

        Task<Bookin?> UpdateBooknigAsync(Booking booking);

        Task<bool> DeleteBookingAsync(int id); // This does not sound familiar to me. We did it in a different way (more beginner friendly?)

        // Returns all bookings for a specific date
        Task<IEnumerable<Booknig>> GetBookingsForDayAsync(DateOnly date);

        // Returs booking counts per court and total between two dates 
        Task<IEnumerable<CourtStatisticsDto>> GetStatisticsAsync(DateOnly startDate, DateOnly endDate);

        // Returns available time slots for all courts between two dates 
        Task<IEnumerable<AvailableSlotDto>> GetAvailabilityAsync(DateOnly startDate, DateOnly endDate);

    }
}