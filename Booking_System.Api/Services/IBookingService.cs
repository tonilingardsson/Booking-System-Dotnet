using Booking_System.Api.Dtos;
using Booking_System.Models;

namespace Booking_System.Api.Services
{
    // Interface = contract 
    // The controller will only know about this interface, not the concrete class
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        
        Task<Booking?> GetBookingByIdAsync(int id);

        // Creates a new booking after validating business rules
        // Throws BookingValidationException if any rule is broken
        Task<Booking> CreateBookingAsync(Booking booking);

        Task<Booking?> UpdateBookingAsync(Booking booking);

        Task<bool> DeleteBookingAsync(int id);

        // -----------------------------
        // User story / admin queries
        // -----------------------------

        // Returns all bookings for a specific date
        Task<IEnumerable<Booking>> GetBookingsForDayAsync(DateOnly date);

        // Returs booking counts per court and total between two dates 
        Task<IEnumerable<CourtStatisticsDto>> GetStatisticsAsync(DateOnly startDate, DateOnly endDate);

        // Returns available time slots for all courts between two dates 
        Task<IEnumerable<AvailableSlotDto>> GetAvailabilityAsync(DateOnly startDate, DateOnly endDate);
        // Task GetAvailabilityAsync(DateTime startDate, DateTime endDate);
    }
}