

using MovieBooking.Models;

namespace MovieBooking.Repository.IRepository
{
    public interface ITheatreRepository : IRepository<Theatre>
    {
       void Update(Theatre obj);

    }
}
