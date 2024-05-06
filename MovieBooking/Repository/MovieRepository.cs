
using MovieBooking.Models;
using MovieBooking.Repository.IRepository;

namespace MovieBooking.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private MovieEntites _db;
        public MovieRepository(MovieEntites db) : base(db)
        {
            _db = db;
        }
        public void Update(Movie obj)
        {
           _db.Update(obj);
        }

    }
}
