using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using TendersLib;

namespace MVC.Controllers
{


    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            var byteArray = System.Text.Encoding.ASCII.GetBytes("mvcuser:mvcpassword");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.ShowHeader = false;
                var filesResponse = await _httpClient.GetAsync("http://localhost:5000/api/tenders/files");
                filesResponse.EnsureSuccessStatusCode();
                var filesContent = await filesResponse.Content.ReadAsStringAsync();
                var files = JsonSerializer.Deserialize<List<string>>(filesContent);
                ViewBag.Files = files;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                ViewBag.Files = new List<string>(); 
            }
            return View();
        }

        public async Task<IActionResult> GetData(string fileName)
        {
            try
            {
                var filesResponse = await _httpClient.GetAsync("http://localhost:5000/api/tenders/files");
                filesResponse.EnsureSuccessStatusCode();
                var filesContent = await filesResponse.Content.ReadAsStringAsync();
                var files = JsonSerializer.Deserialize<List<string>>(filesContent);
                ViewBag.Files = files;

                var dataResponse = await _httpClient.GetAsync($"http://localhost:5000/api/tenders/data/{fileName}");
                dataResponse.EnsureSuccessStatusCode();
                var dataContent = await dataResponse.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<List<Tender>>(dataContent);
                ViewBag.FileName = fileName;
                return View(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(new List<Tender>()); 
            }
        }
    }




}
