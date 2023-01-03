using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using gabinet_rejestracja.Models;
using Newtonsoft.Json;

namespace gabinet_rejestracja.Controllers
{
    public class DoctorController : Controller
    {
        private static HttpClient httpMsgClient = new HttpClient();
        public DoctorController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }



        //CREATE
        [HttpGet]
        public async Task<IActionResult> Doctor()
        {
            ViewBag.LogMessage = HttpContext.Session.GetString("UserName");
            await Task.Delay(1000);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Doctor(Doctor doctorReq)
        {
            if (ModelState.IsValid)
            {
                var serializedProductToCreate = JsonConvert.SerializeObject(doctorReq);
                var request = new HttpRequestMessage(HttpMethod.Post, Configuration.GetValue<string>("WebAPIBaseUrl") + "/doctor");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(serializedProductToCreate);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await httpMsgClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("DoctorMessage", "Messages");
                }
                else
                {
                    return RedirectToAction("Load1", "Error");
                }
            }
            else
                return RedirectToAction("Load2", "Error");
        }



        //INDEX
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            httpMsgClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpMsgClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
            var response = await httpMsgClient.GetAsync(Configuration.GetValue<string>("WebAPIBaseUrl") + "/doctor");
            var content = await response.Content.ReadAsStringAsync();

            ViewBag.LogMessage = HttpContext.Session.GetString("UserName");

            if (response.IsSuccessStatusCode)
            {
                var doctorReq = new List<Doctor>();
                if (response.Content.Headers.ContentType.MediaType == "application/json")
                {
                    doctorReq = JsonConvert.DeserializeObject<List<Doctor>>(content);
                }
                return View(doctorReq);
            }
            else
            {
                return RedirectToAction("Load1", "Error");
            }
        }

    }
}