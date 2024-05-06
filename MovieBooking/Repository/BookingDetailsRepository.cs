
using MovieBooking.Models;
using MovieBooking.Repository.IRepository;

namespace MovieBooking.Repository
{
    public class BookingDetailsRepository : Repository<BookingDetails>, IBookingDetailsRepository
    {
        private MovieEntites _db;
        public BookingDetailsRepository(MovieEntites db) : base(db)
        {
            _db = db;
        }
        public void Update(BookingDetails obj)
        {
           _db.Update(obj);
        }

    }
}
