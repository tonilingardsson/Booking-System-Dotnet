using System.ComponentModel.DataAnnotations.Schema;

namespace Booking_System.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int CourtId { get; set; }
        public DateTime StartTime { get; set; }

        [NotMapped]
        public DateTime EndTime => StartTime.AddHours(1);

        public Customer? Customer { get; set; }
        public Court? Court { get; set; }
    }
}