using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodAdvisor.Models;
using Microsoft.EntityFrameworkCore;
using static FoodAdvisor.Queries.RestaurantQueries;

namespace FoodAdvisor.Services
{
    public class RestaurantServices
    {
        private readonly RestaurantContext _context;

        public RestaurantServices()
        {
            _context = new RestaurantContext();
        }

        /// <summary>
        /// Gets all the restaurants.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Restaurant>> GetAll()
        {
            return await _context.Restaurants.Include(r => r.Address)
                                             .Include(r => r.Grade)
                                             .ToListAsync();
        }

        public async Task<List<Restaurant>> GetBySearch(Dictionary<SearchCategory, string> search)
        {
            var restaurants = await _context.Restaurants.Include(r => r.Address)
                                                                      .Include(r => r.Grade)
                                                                      .ToListAsync();
            if (!string.IsNullOrEmpty(search[SearchCategory.Name]))
            {
                restaurants = restaurants.RestaurantsBySearchName(search[SearchCategory.Name].ToLower());
            }
            else if (!string.IsNullOrEmpty(search[SearchCategory.Address]))
            {
                restaurants = restaurants.RestaurantsBySearchAddress(search[SearchCategory.Address].ToLower());
            }
            else if (!string.IsNullOrEmpty(search[SearchCategory.Score]))
            {
                restaurants = restaurants.RestaurantsBySearchScore(search[SearchCategory.Score].ToLower());
            }

            return restaurants;
        }

        /// <summary>
        /// Gets the specified restaurant.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Restaurant> Get(int? id)
        {
            return await _context.Restaurants.Include(r => r.Address)
                                             .Include(r => r.Grade)
                                             .SingleOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Restaurant>> SetPositionsRestaurants()
        {
            var restaurants = await _context.Restaurants.Include(r => r.Address)
                                                                      .Include(r => r.Grade)
                                                                      .ToListAsync();
            restaurants = restaurants.OrderByDescendingRestaurants();

            for (int i = 0; i < restaurants.Count; i++)
            {
                restaurants[i].Position = i + 1;
            }

            return await UpdateList(restaurants);
        }

        public async Task<List<Restaurant>> UpdateList(List<Restaurant> restaurants)
        {
            foreach (var resto in restaurants)
            {
                _context.Entry(resto).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
            return restaurants;
        }

        public async Task<List<Restaurant>> GetBestRestaurants(int? number)
        {
            var restaurants = await SetPositionsRestaurants();

            if (number != null)
            {
                restaurants = restaurants.BestRestaurants((int)number);
            }

            return restaurants;
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

        /// <summary>
        /// Determines whether the specified identifier is exists.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified identifier is exists; otherwise, <c>false</c>.
        /// </returns>
        public bool IsExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
