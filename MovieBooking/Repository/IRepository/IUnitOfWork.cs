

namespace MovieBooking.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ITheatreRepository TheatreRepository { get; }
        IMovieRepository  MovieRepository { get; }
        IScreenRepository ScreenRepository { get; } 
        IBookingDetailsRepository BookingDetailsRepository { get; }
        void Save();
    }
}
