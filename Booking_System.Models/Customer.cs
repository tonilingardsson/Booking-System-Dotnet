using System.ComponentModel.DataAnnotations;

namespace Booking_System.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [MinLength(2, ErrorMessage = "Minimum of 2 characters is required")]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [MinLength(2, ErrorMessage = "Minimum of 2 characters is required")]
        [MaxLength(50, ErrorMessage = "Maximum of 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string EmailAddress { get; set; } string.Empty;

        [Phone(ErrorMessage = "Invalid PhoneNumber format")]
        public string PhoneNumber { get; set; } string.Empty;
    }
}
