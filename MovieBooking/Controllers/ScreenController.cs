
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBooking.Models;
using MovieBooking.Repository.IRepository;
using MovieBooking.ViewModel;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;
using System.Text;


namespace MovieBooking.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ScreenController : Controller

    {
        string Baseurl = "https://localhost:7053/api/";
        private readonly IUnitOfWork _unitOfWork;
        public ScreenController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> ScreenList()
        {
            List<ScreenViewModel> data = new List<ScreenViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi/ScreenList");
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<ScreenViewModel>>(UserResponse);
                }

                return Json(data);
            }
        }

        [HttpGet]
        public JsonResult TheatreList()
        {
            var theatreList = _unitOfWork.TheatreRepository
                .GetAll()
                .Select(u => new { u.TheatreId, u.TheatreName })
                .ToList();

            return Json(theatreList);
        }


        [HttpGet]
        public JsonResult MovieList()
        {
            var movies = _unitOfWork.MovieRepository.GetAll().Select(u => new { u.MovieName, u.MovieId }).ToList();

            return Json(movies);
        }


        [HttpPost]
        public async Task<JsonResult> AddScreen(Screen screen)
        {
            try
            {
                if (ModelState.IsValid)
                {
            

                    var obj = new ScreenViewModel
                    {
                        ScreenId = screen.ScreenId,
                        MovieId = screen.MovieId,
                        TheatreId = screen.TheatreId,
                        TotalSeats = screen.TotalSeats,
                        ShowTime = new List<DateTime> { screen.Showtime },

                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(obj); // Serialize object to JSON
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("UserApi/ScreenAdd", content);

                        if (response.IsSuccessStatusCode)
                        {
                            return new JsonResult(new { success = true });
                        }
                        else
                        {
                            // Log or handle unsuccessful response
                            return new JsonResult(new { success = false });
                        }
                    }
                }
                else
                {
                    // Log or handle invalid model state
                    return new JsonResult(new { success = false });
                }
            }
            catch (Exception ex)
            {
                // Log or handle exception
                return new JsonResult(new { success = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditScreen(int id)
        {
            Screen screen = new Screen();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"UserApi/GetScreen/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    screen = JsonConvert.DeserializeObject<Screen>(result);
                }
            }
            return Json(screen);
        }


        [HttpPost]
        public async Task<JsonResult> EditScreen(ScreenViewModel screenViewModel)
        {
            if (ModelState.IsValid)
            {
                var obj = new ScreenViewModel
                {
                    Id = screenViewModel.Id,
                    ScreenId = screenViewModel.ScreenId,
                    MovieId = screenViewModel.MovieId,
                    TheatreId = screenViewModel.TheatreId,
                    TotalSeats = screenViewModel.TotalSeats,
                    ShowTime = screenViewModel.ShowTime
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"UserApi/UpdateScreen/{screenViewModel.ScreenId}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true });
                    }
                }
            }

            return Json(new { success = false, message = "Please enter required fields" });
        }



        public async Task<JsonResult> Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"UserApi/DeleteScreen/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, message = "Failed to delete driver" });
        }
    }
}
