using System;

namespace Booking_System.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        public Customer? Customer { get; set; } = null!;
        
        public int CourtId { get; set; }
        
        public Court? Court { get; set; } = null!;

        public DateTime StartTime {  get; set; }
    }
}