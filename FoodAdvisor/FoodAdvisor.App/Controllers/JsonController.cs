using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using FoodAdvisor.App.Models;
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

                ViewBag.ExportMessageSuccess = "The data has been successfully exported. 👍";
                ViewBag.ExportMessageError = "";
            }
            else
            {
                ViewBag.ExportMessageSuccess = "";
                ViewBag.ExportMessageError = "An error occured when try to export the data. 😦";
            }

            return View("Index");
        }

        public IActionResult Import(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                new RestaurantJson().Import(collection["path"]);

                ViewBag.ImportMessageSuccess = "The data has been successfully imported. 👍";
                ViewBag.ImportMessageError = "";
            }
            else
            {
                ViewBag.ImportMessageSuccess = "";
                ViewBag.ImportMessageError = "An error occured when try to import the data. 😦";
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}