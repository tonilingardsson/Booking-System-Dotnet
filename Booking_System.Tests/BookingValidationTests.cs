using Booking_System.Api.Data;
using Booking_System.Api.Services;
using Booking_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace Booking_System.Tests
{
    [TestClass]
    public class BookingValidationTests
    {
        private BookingDbContext _context = null!;
        private BookingService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new BookingDbContext(options);

            _context.Customers.Add(new Customer
            {
                Id = 1,
                FirstName = "Antonio",
                LastName = "Luna",
                EmailAddress = "antonio@luna.com",
                PhoneNumber = "0729291305"
            });

            _context.Courts.Add(new Court
            {
                Id = 1,
                CourtName = "Rafa Nadal"
            });

            _context.Courts.Add(new Court
            {
                Id = 2,
                CourtName = "Carlos Alcaraz"
            });

            _context.SaveChanges();

            _service = new BookingService(_context);
        }

        [TestMethod]
        public async Task CreateBooking_ValidAtOpeningHour_Succeeds()
        {
            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 7, 0, 0)
            };

            var result = await _service.CreateBookingAsync(booking);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.CustomerId);
            Assert.AreEqual(1, result.CourtId);
            Assert.AreEqual(7, result.StartTime.Hour);
        }

        [TestMethod]
        public async Task CreateBooking_ValidAtLastHour_Succeeds()
        {
            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 21, 0, 0)
            };

            var result = await _service.CreateBookingAsync(booking);

            Assert.IsNotNull(result);
            Assert.AreEqual(21, result.StartTime.Hour);
        }

        [TestMethod]
        public async Task CreateBooking_BeforeOpeningHours_ThrowsException()
        {
            // Arrange
            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 6, 0, 0)
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BookingValidationException>(() => _service.CreateBookingAsync(booking));
                
            Assert.AreEqual("Bookings are only allowed between 7:00 and 22:00.", exception.Message);

        }

        [TestMethod]
        public async Task CreateBooking_AfterClosingHours_ThrowsException()
        {
            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 22, 0, 0)
            };

    
                var exception = await Assert.ThrowsAsync<BookingValidationException>(()=> _service.CreateBookingAsync(booking));
                Assert.Fail("Expected BookingValidationException was not thrown.");
            
                Assert.AreEqual("Bookings are only allowed between 7:00 and 22:00.", exception.Message);
            
        }

        [TestMethod]
        public async Task CreateBooking_NotWholeHour_ThrowsException()
        {
            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 13, 30, 0)
            };

            var exception = await Assert.ThrowsAsync<BookingValidationException>(()=>_service.CreateBookingAsync(booking));
                Assert.Fail("Expected BookingValidationException was not thrown.");
            
                Assert.AreEqual("Bookings must start on whole hours (e.g. 13:00).", exception.Message);
            
        }

        [TestMethod]
        public async Task CreateBooking_NonExistingCustomer_ThrowsException()
        {
            var booking = new Booking
            {
                CustomerId = 999,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            };

            var exception = await Assert.ThrowsAsync<BookingValidationException>(()=>_service.CreateBookingAsync(booking));
                Assert.Fail("Expected BookingValidationException was not thrown.");
            
                Assert.AreEqual("Customer does not exist.", exception.Message);
            
        }

        [TestMethod]
        public async Task CreateBooking_NonExistingCourt_ThrowsException()
        {
            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 999,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            };

            try
            {
                await _service.CreateBookingAsync(booking);
                Assert.Fail("Expected BookingValidationException was not thrown.");
            }
            catch (BookingValidationException ex)
            {
                Assert.AreEqual("Court does not exist.", ex.Message);
            }
        }

        [TestMethod]
        public async Task CreateBooking_DuplicateCourtAndTime_ThrowsException()
        {
            _context.Bookings.Add(new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            });
            _context.SaveChanges();

            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            };

            try
            {
                await _service.CreateBookingAsync(booking);
                Assert.Fail("Expected BookingValidationException was not thrown.");
            }
            catch (BookingValidationException ex)
            {
                Assert.AreEqual("This court is already booked at that time.", ex.Message);
            }
        }

        [TestMethod]
        public async Task CreateBooking_SameTimeDifferentCourt_Succeeds()
        {
            _context.Bookings.Add(new Booking
            {
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            });
            _context.SaveChanges();

            var booking = new Booking
            {
                CustomerId = 1,
                CourtId = 2,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            };

            var result = await _service.CreateBookingAsync(booking);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.CourtId);
        }

        [TestMethod]
        public async Task UpdateBooking_SameBooking_IgnoresOwnId_Succeeds()
        {
            var existingBooking = new Booking
            {
                Id = 1,
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            };

            _context.Bookings.Add(existingBooking);
            _context.SaveChanges();

            var bookingToUpdate = new Booking
            {
                Id = 1,
                CustomerId = 1,
                CourtId = 1,
                StartTime = new DateTime(2026, 6, 23, 10, 0, 0)
            };

            var result = await _service.UpdateBookingAsync(bookingToUpdate);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}