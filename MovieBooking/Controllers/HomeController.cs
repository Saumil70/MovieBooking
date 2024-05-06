
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models;
using MovieBooking.Repository.IRepository;
using MovieBooking.ViewModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace MovieBooking.Controllers 
{ 

    public class HomeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Movie> movielist = _unitOfWork.MovieRepository.GetAll();
            return View(movielist);
        }

        [Authorize]
        public IActionResult Details(int movieId)
        {
            var screens = _unitOfWork.ScreenRepository.GetAll(u => u.MovieId == movieId, includeProperties: "Movie,Theatre");

            if (screens == null || !screens.Any())
            {
                return NotFound();
            }

       
            var screenViewModels = screens.Select(screen => new ScreenViewModel
            {
                Id = screen.Id, 
                ScreenId = screen.ScreenId,
                MovieId = screen.MovieId,
                TheatreId = screen.TheatreId,
                TotalSeats = screen.TotalSeats,
                ShowTime = new List<DateTime> { screen.Showtime },
                Movie = screen.Movie,
                Theatre = screen.Theatre,
                RemainingSeats = screen.RemainingSeats
                
            }).ToList();

            return View(screenViewModels);
        }

        [Authorize]
        public async Task<IActionResult> Booking(int id, int Quantity)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var screens = _unitOfWork.ScreenRepository.Get(u => u.Id == id, includeProperties: "Movie,Theatre");

            var obj = new BookingDetails
            {
                TotalSeatSelected = Quantity,
                Amount = 200 * Quantity,
                MovieId = screens.MovieId,
                TheatreId = screens.TheatreId,
                Id = screens.Id,
                UserId = user?.Id,
            };

            try
            {

                _unitOfWork.BookingDetailsRepository.Add(obj);
                _unitOfWork.Save();

                var existingScreen = _unitOfWork.ScreenRepository.Get(u => u.Id == id);
                existingScreen.RemainingSeats -= Quantity;
                _unitOfWork.ScreenRepository.Update(existingScreen);
                _unitOfWork.Save();

                var booking = new BookingDetailsViewModel
                {
                    TotalSeatSelected = Quantity,
                    Amount = 200 * Quantity,
                    MovieId = screens.MovieId,
                    TheatreId = screens.TheatreId,
                    Id = screens.Id,
                    UserId = user?.Id,
                    Theatre = screens.Theatre,
                    Movie = screens.Movie,
                    Screen = screens
                };

                // Return success status and data
                return Json(new { success = true, data = booking });
            }
            catch (Exception ex)
            {
                // Return error status
                return Json(new { success = false, error = ex.Message });
            }
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
