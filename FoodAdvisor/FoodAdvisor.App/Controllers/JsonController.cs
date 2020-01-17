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
        private RestaurantJson _json;

        public JsonController()
        {
            _services = new RestaurantServices();
            _json = new RestaurantJson();
        }

        public IActionResult Index()
        {
            // default value for import and export
            ViewBag.UrlImport = @".\Resources\restaurants.net.json";
            ViewBag.UrlExport = @".\Resources\exportFile.net.json";
            return View();
        }

        public IActionResult Export(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                // get all the restaurants
                var restaurants = _services.GetAll().Result;

                // write it like a json in the specified file
                var isOk = _json.WriteFile(restaurants, collection["path"]);

                // if the export works
                if (isOk)
                {
                    // display a success message
                    ViewBag.ExportMessageSuccess = "The data has been successfully exported. 👍";
                    ViewBag.ExportMessageError = "";
                }
                else
                {
                    // display an error message
                    ViewBag.ExportMessageSuccess = "";
                    ViewBag.ExportMessageError = "An error occured when try to export the data. 😦";
                }
            }
            else
            {
                // if the model is not valid, display an error message
                ViewBag.ExportMessageSuccess = "";
                ViewBag.ExportMessageError = "An error occured when try to export the data. 😦";
            }

            // display the good url
            ViewBag.UrlExport = collection["path"];
            ViewBag.UrlImport = @".\Resources\restaurants.net.json";

            return View("Index");
        }

        public IActionResult Import(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                // import the data in the database of the json of the specified file
                var isOk = _json.Import(collection["path"]).Result;

                // if the import works
                if (isOk)
                {
                    // display a success message
                    ViewBag.ImportMessageSuccess = "The data has been successfully imported. 👍";
                    ViewBag.ImportMessageError = "";
                }
                else
                {
                    // display an error message
                    ViewBag.ImportMessageSuccess = "";
                    ViewBag.ImportMessageError = "An error occured when try to import the data. 😦";
                }
            }
            else
            {
                // if the model is not valid, display an error message
                ViewBag.ImportMessageSuccess = "";
                ViewBag.ImportMessageError = "An error occured when try to import the data. 😦";
            }

            // display the good url
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