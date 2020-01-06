using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodAdvisor.App.Models;
using FoodAdvisor.Services;

namespace FoodAdvisor.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RestaurantServices _services;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _services = new RestaurantServices();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _services.GetBestRestaurants(5));
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
