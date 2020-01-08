using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodAdvisor.Models;
using FoodAdvisor.Services;
using Microsoft.AspNetCore.Http;

namespace FoodAdvisor.App.Controllers
{
    public class RestaurantsController : Controller
    {
        private RestaurantServices _services;

        public RestaurantsController()
        {
            _services = new RestaurantServices();
        }

        // GET: Restaurants
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            ViewBag.Name = collection[SearchCategory.Name.ToString()];
            ViewBag.Address = collection[SearchCategory.Address.ToString()];
            ViewBag.Score = collection[SearchCategory.Score.ToString()];
            Dictionary<SearchCategory, string> search = new Dictionary<SearchCategory, string>
            {
                { SearchCategory.Name, ViewBag.Name },
                { SearchCategory.Address, ViewBag.Address },
                { SearchCategory.Score, ViewBag.Score }
            };

            if (search.Any(pair => !string.IsNullOrEmpty(pair.Value)))
            {
                return View(await _services.GetBySearch(search));
            }

            return View(await _services.GetAll());

        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _services.Get(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, [Bind("Id,Name,Phone,Comment,MailOwner")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var resto = new Restaurant
                {
                    Name = collection["Name"],
                    Phone = collection["Phone"],
                    Comment = collection["Comment"],
                    MailOwner = collection["MailOwner"],

                    Address = new Address
                    {
                        Street = collection["Address.Street"],
                        City = collection["Address.City"],
                        ZipCode = collection["Address.ZipCode"]
                    },

                    Grade = new Grade
                    {
                        Date = Convert.ToDateTime(collection["Grade.Date"]),
                        Score = int.Parse(collection["Grade.Score"]),
                        Comment = collection["Grade.Comment"]
                    }
                };

                _ = await _services.Add(resto);
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Edit(int id, IFormCollection collection, [Bind("Id,Name,Phone,Comment,MailOwner")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var r = await _services.Get(id);
                    r.Name = collection["Name"];
                    r.Phone = collection["Phone"];
                    r.Comment = collection["Comment"];
                    r.MailOwner = collection["MailOwner"];
                    r.Address.Street = collection["Address.Street"];
                    r.Address.City = collection["Address.City"];
                    r.Address.ZipCode = collection["Address.ZipCode"];
                    r.Grade.Date = Convert.ToDateTime(collection["Grade.Date"]);
                    r.Grade.Score = int.Parse(collection["Grade.Score"]);
                    r.Grade.Comment = collection["Grade.Comment"];

                    await _services.Update(r);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var restaurant = await _services.Get(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _services.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _services.IsExists(id);
        }
    }
}
