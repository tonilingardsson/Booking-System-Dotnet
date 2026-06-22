using System.ComponentModel.DataAnnotations;

namespace Booking_System.Models
{
    public class Court
    {
        public int Id { get; set; }

        [MinLength(2, ErrorMessage = "Minimum of 2 characters is required")]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters")]
        public string CourtName { get; set; } = string.Empty;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
