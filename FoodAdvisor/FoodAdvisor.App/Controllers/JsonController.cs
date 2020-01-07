using System;
using System.IO;
using System.Text;
using FoodAdvisor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodAdvisor.App.Controllers
{
    public class JsonController : Controller
    {
        private RestaurantServices _services;

        public JsonController()
        {
            _services = new RestaurantServices();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Export(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var restaurants = new RestaurantServices().GetAll().Result;
                new RestaurantJson().WriteFile(restaurants, collection["path"]);
            }

            return RedirectToAction("Index", "Restaurants");
        }

        public IActionResult Import(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                new RestaurantJson().Import(collection["path"]);
            }

            return RedirectToAction("Index", "Restaurants");
        }
    }
}