
using MovieBooking.Models;

namespace MovieBooking.Repository.IRepository
{
    public interface IScreenRepository : IRepository<Screen>
    {
        void Update(Screen obj);
      


    }
}
