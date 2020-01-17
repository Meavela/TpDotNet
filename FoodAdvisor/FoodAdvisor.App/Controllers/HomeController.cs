using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodAdvisor.App.Models;
using FoodAdvisor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            // return a list of 5 best restaurants
            return View(await _services.GetBestRestaurants(5));
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get the information of the restaurant
            var restaurant = await _services.Get(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, IFormCollection collection)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                // get the information of the specified id
                var r = await _services.Get(id);
                // modify with the fields of the form
                r.Grade.Date = Convert.ToDateTime(collection["Grade.Date"]);
                r.Grade.Score = int.Parse(collection["Grade.Score"]);
                r.Grade.Comment = collection["Grade.Comment"];

                // update the restaurant modify
                await _services.Update(r);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists((int)id))
                {
                    return NotFound();
                }

                throw;
            }

            // redirect to the list of 5 best restaurants
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// check if the Restaurant exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool RestaurantExists(int id)
        {
            return _services.IsExists(id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
