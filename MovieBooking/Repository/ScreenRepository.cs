



using MovieBooking.Models;
using MovieBooking.Repository.IRepository;

namespace MovieBooking.Repository
{
    public class ScreenRepository : Repository<Screen>, IScreenRepository
    {
        private MovieEntites _db;
        public ScreenRepository(MovieEntites db) : base(db)
        {
            _db = db;
        }
        public void Update(Screen obj)
        {
           _db.Update(obj);
        }



    }
}
