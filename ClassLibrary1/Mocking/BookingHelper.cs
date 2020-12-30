using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary1.Mocking
{
    public static class BookingHelper
    {
        // this class is static that probably used in different places
        // so we should use dependency by parameters injection without costly modification
        public static string OverlappingBookingsExist(Booking booking, IBookingRepository repository)
        {
            if (booking.Status == "Cancelled")
                return string.Empty;

            var bookings = repository.GetActiveBookings(booking.Id);
            // following few lines are logic for overlapping bookings
            // so it is properly inside this method
            var overlappingBooking = bookings.FirstOrDefault(
                b =>
                    booking.ArrivalDate < b.DepartureDate
                    && b.ArrivalDate < booking.DepartureDate);
            // https://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
            // bool overlap = a.start < b.end && b.start < a.end;

            return overlappingBooking == null ? string.Empty : overlappingBooking.Reference;
        }
    }

    public class UnitOfWork
    {
        public IQueryable<T> Query<T>()
        {
            return new List<T>().AsQueryable();
        }
    }

    public class Booking
    {
        public string Status { get; set; }
        public int Id { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public string Reference { get; set; }
    }
}