using Microsoft.EntityFrameworkCore;
using Booking_System.Models;

namespace Booking_System.Api.Data
{
    public class BookingDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed the three courts required by the assignment
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

            // Seed a couple of customers for testing bookings
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Antonio",
                    LastName = "Gonzalez",
                    EmailAddress = "antonio.gonzalez@gmail.com",
                    PhoneNumber = "123-456-7890"
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Maria",
                    LastName = "Rodriguez",
                    EmailAddress = "maria.rodriguez@gmail.com",
                    PhoneNumber = "098-765-4321"
                }
            );
        }
    }
}