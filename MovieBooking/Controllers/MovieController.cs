
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class MovieController : Controller

    {
        string Baseurl = "https://localhost:7053/api/";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
    
        public MovieController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> MovieList()
        {
            List<MovieViewModel> data = new List<MovieViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi/MovieList");
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<MovieViewModel>>(UserResponse);
                }

                return Json(data);
            }
        }


        [HttpPost]
        public async Task<JsonResult> AddMovie(Movie movie, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var uploads = Path.Combine(wwwRootPath, "Images");
                        var filePath = Path.Combine(uploads, fileName);


                        if (!string.IsNullOrEmpty(movie.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, movie.ImageUrl.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        movie.ImageUrl = @"\Images\" + fileName;
                    }

                    var obj = new MovieViewModel
                    {
                        MovieName = movie.MovieName,
                        Genre = movie.Genre,
                        Time = movie.Time,
                        ImageUrl = movie.ImageUrl
                       
                        
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(obj); // Serialize object to JSON
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("UserApi/MovieAdd", content);

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
        public async Task<IActionResult> EditMovie(int id)
        {
            Movie movie = new Movie();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"UserApi/GetMovieById/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    movie = JsonConvert.DeserializeObject<Movie>(result);
                }
            }
            return Json(movie);
        }


        [HttpPost]
        public async Task<JsonResult> EditMovie(MovieViewModel movieViewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var uploads = Path.Combine(wwwRootPath, "Images");
                    var filePath = Path.Combine(uploads, fileName);


                    if (!string.IsNullOrEmpty(movieViewModel.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, movieViewModel.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    movieViewModel.ImageUrl = @"\Images\" + fileName;
                }


                var obj = new Movie
                {
                    MovieId = movieViewModel.MovieId,
                    MovieName = movieViewModel.MovieName,
                    Genre = movieViewModel.Genre,
                    Time = movieViewModel.Time,
                    ImageUrl = movieViewModel.ImageUrl
                 
                    
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"UserApi/UpdateMovie/{movieViewModel.MovieId}", content);
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

                HttpResponseMessage response = await client.DeleteAsync($"UserApi/DeleteMovie/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, message = "Failed to delete driver" });
        }
    }
}
