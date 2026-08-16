using System.ComponentModel.DataAnnotations;

namespace Booking_System.Api.Dtos
{
    public class BookingDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "A valid customer ID is required.")]
        public int CustomerId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A valid court ID is required.")]
        public int CourtId { get; set; }

        [Required(ErrorMessage = "A start time is required.")]
        public DateTime? StartTime { get; set; }
    }
}