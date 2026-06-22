namespace Booking_System.Api.Dtos
{
    // Represents one available time slot on a specific court
    // Used to answer the customer's question:
    // "What times are free for all courts in this date range?"
    public class AvailableSlotDto
    {
        public int CourtId { get; set; }
        public string CourName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}