using System;
using System.Diagnostics;
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
            ViewBag.UrlImport = @".\Resources\restaurants.net.json";
            ViewBag.UrlExport = @".\Resources\exportFile.net.json";
            return View();
        }

        public IActionResult Export(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var restaurants = new RestaurantServices().GetAll().Result;
                var isOk = new RestaurantJson().WriteFile(restaurants, collection["path"]);

                if (isOk)
                {
                    ViewBag.ExportMessageSuccess = "The data has been successfully exported. 👍";
                    ViewBag.ExportMessageError = "";
                }
                else
                {
                    ViewBag.ExportMessageSuccess = "";
                    ViewBag.ExportMessageError = "An error occured when try to export the data. 😦";
                }
            }
            else
            {
                ViewBag.ExportMessageSuccess = "";
                ViewBag.ExportMessageError = "An error occured when try to export the data. 😦";
            }

            ViewBag.UrlExport = collection["path"];
            ViewBag.UrlImport = @".\Resources\restaurants.net.json";

            return View("Index");
        }

        public IActionResult Import(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var isOk = new RestaurantJson().Import(collection["path"]).Result;
                if (isOk)
                {
                    ViewBag.ImportMessageSuccess = "The data has been successfully imported. 👍";
                    ViewBag.ImportMessageError = "";
                }
                else
                {
                    ViewBag.ImportMessageSuccess = "";
                    ViewBag.ImportMessageError = "An error occured when try to import the data. 😦";
                }
            }
            else
            {
                ViewBag.ImportMessageSuccess = "";
                ViewBag.ImportMessageError = "An error occured when try to import the data. 😦";
            }

            ViewBag.UrlImport = collection["path"];
            ViewBag.UrlExport = @".\Resources\exportFile.net.json";

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}