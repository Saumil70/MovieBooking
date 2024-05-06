
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
    public class TheatreController : Controller

    {
        string Baseurl = "https://localhost:7053/api/";
        private readonly IUnitOfWork _unitOfWork;
        public TheatreController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> TheatreList()
        {
            List<TheatreViewModel> data = new List<TheatreViewModel>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("UserApi");
                if (res.IsSuccessStatusCode)
                {
                    var UserResponse = await res.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<List<TheatreViewModel>>(UserResponse);
                }

                return Json(data);
            }
        }


        [HttpPost]
        public async Task<JsonResult> AddTheatre(Theatre theatre)
        {
            try
            {
                if (ModelState.IsValid)
                {
            

                    var obj = new TheatreViewModel
                    {
                        TheatreName = theatre.TheatreName,
                        Address = theatre.Address,
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var json = JsonConvert.SerializeObject(obj); // Serialize object to JSON
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync("UserApi", content);

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
        public async Task<IActionResult> Edit(int id)
        {
            Theatre theatre = new Theatre();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"UserApi/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    theatre = JsonConvert.DeserializeObject<Theatre>(result);
                }
            }
            return Json(theatre);
        }


        [HttpPost]
        public async Task<JsonResult> EditTheatre(TheatreViewModel theatreViewModel)
        {
            if (ModelState.IsValid)
            {
                var obj = new Theatre
                {
                    TheatreId = theatreViewModel.TheatreId,
                    TheatreName = theatreViewModel.TheatreName,
                    Address = theatreViewModel.Address,
                };

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(obj);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync($"UserApi/{theatreViewModel.TheatreId}", content);
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

                HttpResponseMessage response = await client.DeleteAsync($"UserApi/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false, message = "Failed to delete driver" });
        }
    }
}
