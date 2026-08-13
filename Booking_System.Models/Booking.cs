using Booking_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Booking
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CourtId { get; set; }
    public DateTime StartTime { get; set; }

    [NotMapped] // EF do not look for this property in the database, it is calculated on the fly
    public DateTime EndTime => StartTime.AddHours(1);
    public Customer? Customer { get; set; }
    public Court? Court { get; set; } = null;
}