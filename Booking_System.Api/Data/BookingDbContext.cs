using Microsoft.EntityFrameworkCore;
using Booking_System.Models;

namespace Booking_System.Api.Data
{
    public class BookingDbContext : DbContext
    {
        // Three DbSet properties only
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Constructor
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Court>().HasData(
                new Court
                {
                    Id = 1,
                    CourtName = "Rafa Nadal"
                },
                new Court
                {
                    Id = 2,
                    CourtName = "Roger Federer"
                 },
                new Court
                {
                    Id = 3,
                    CourtName = "Björn Borg"
                }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Antonio",
                    LastName = "Luna",
                    EmailAddress = "antonio@luna.com",
                    PhoneNumber = "0729291305"
                }
            );
        }
    }
}