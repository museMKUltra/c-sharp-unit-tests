using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Mocking
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }

    public class BookingRepository : IBookingRepository
    {
        public IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null)
        {
            var unitOfWork = new UnitOfWork();

            // you might use active bookings somewhere else
            // so encapsulated following few lines instead of repeat multi-places
            var bookings = unitOfWork.Query<Booking>()
                .Where(b => b.Status != "Cancelled");
            // this is dynamical constructed logic, but for this modification
            // it is more suitable to cover with integration test
            // to prevent breaking logic before run following code
            if (excludedBookingId.HasValue)
                bookings = bookings.Where(b => b.Id != excludedBookingId.Value);

            return bookings;
        }
    }
}