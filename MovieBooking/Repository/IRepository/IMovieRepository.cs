



using MovieBooking.Models;

namespace MovieBooking.Repository.IRepository
{
    public interface IMovieRepository : IRepository<Movie>
    {
        void Update(Movie obj);


    }
}
