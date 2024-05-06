



using MovieBooking.Models;

namespace MovieBooking.Repository.IRepository
{
    public interface  IBookingDetailsRepository : IRepository<BookingDetails>
    {
        void Update(BookingDetails obj);


    }
}
