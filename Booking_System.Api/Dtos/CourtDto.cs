using System.ComponentModel.DataAnnotations;

namespace Booking_System.Dtos
{
    public class CourtDto
    {
        [Required(ErrorMessage = "Court name is required")]
        [MinLength(2, ErrorMessage = "Minimum of 2 characters is required")]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters")]
        public string CourtName { get; set; } = string.Empty;
    }
}