namespace Booking_System.Api.Dtos
{
    // Represents booking count for one court in a date range.
    // Used for the admin story: bookings per count + total.
    public class CourtStatisticsDto
    {
        public int CourtId { get; set; }
        public string CourtName { get; set; } = string.Empty;
        public int BookingCount { get; set; }
    }
}