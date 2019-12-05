using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAdvisor.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAdvisor.Services
{
    public class RestaurantServices
    {
        private readonly RestaurantContext _context;

        public RestaurantServices(RestaurantContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all the restaurants.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Restaurant>> GetAll()
        {
            return await _context.Restaurants.ToListAsync();
        }

        /// <summary>
        /// Gets the specified restaurant.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Restaurant> Get(int? id)
        {
            return await _context.Restaurants.FindAsync(id);
        }

        /// <summary>
        /// Adds the specified restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant.</param>
        /// <returns></returns>
        public async Task<Restaurant> Add(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        /// <summary>
        /// Deletes the specified restaurant.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Restaurant> Delete(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        /// <summary>
        /// Updates the specified restaurant.
        /// </summary>
        /// <param name="restaurant">The restaurant.</param>
        /// <returns></returns>
        public async Task<Restaurant> Update(Restaurant restaurant)
        {
            _context.Entry(restaurant).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public bool IsExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
