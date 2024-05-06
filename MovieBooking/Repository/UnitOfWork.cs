
using Microsoft.AspNetCore.Identity;
using MovieBooking.Models;
using MovieBooking.Repository.IRepository;



namespace MovieBooking.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private MovieEntites _db;
        public ITheatreRepository TheatreRepository { get; private set; }

        public IMovieRepository MovieRepository { get; private set; }

        public IScreenRepository ScreenRepository { get; private set; }
        public IBookingDetailsRepository BookingDetailsRepository { get; private set; }





        public UnitOfWork(MovieEntites db)
        {
            _db = db;
            TheatreRepository = new TheatreRepository(_db);
            MovieRepository = new MovieRepository(_db);
            ScreenRepository = new  ScreenRepository(_db);
            BookingDetailsRepository = new BookingDetailsRepository(_db);

        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
