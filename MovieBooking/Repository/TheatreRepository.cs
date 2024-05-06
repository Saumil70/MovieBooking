using Microsoft.EntityFrameworkCore;
using MovieBooking.Models;
using MovieBooking.Repository.IRepository;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;



namespace MovieBooking.Repository
{
    public class TheatreRepository : Repository<Theatre>, ITheatreRepository
    {
        private MovieEntites _db;
        public TheatreRepository(MovieEntites db) : base(db)
        {
            _db = db;
        }


        public void Update(Theatre obj)
        {
            _db.Update(obj);
        }




    }
}
