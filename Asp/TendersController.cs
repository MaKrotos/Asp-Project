
using Asp.Excel;
using ClassesWeb;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Core_Web_API
{
    public class TendersController : Controller
    {
        private readonly HttpClient _httpClient;

        public TendersController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("http://your_rest_service_url/api/tenders/files");
            response.EnsureSuccessStatusCode();

            var files = await DeserializeResponse<List<string>>(response);
            return View(files);
        }

        public async Task<IActionResult> Details(string fileName)
        {
            var response = await _httpClient.GetAsync($"http://your_rest_service_url/api/tenders/{fileName}");
            response.EnsureSuccessStatusCode();

            var tenders = await DeserializeResponse<List<Tender>>(response);
            return View(tenders);
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }


}
