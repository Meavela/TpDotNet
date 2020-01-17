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
            // get the list of the restaurants with their address and grade
            return await _context.Restaurants.Include(r => r.Address)
                                             .Include(r => r.Grade)
                                             .ToListAsync();
        }

        /// <summary>
        /// Gets the restaurants by search.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public async Task<List<Restaurant>> GetBySearch(Dictionary<SearchCategory, string> search)
        {
            // get the list of the restaurants with their address and grade
            var restaurants = await _context.Restaurants.Include(r => r.Address)
                                                                      .Include(r => r.Grade)
                                                                      .ToListAsync();
            // modify the list when a user search a specified name
            if (!string.IsNullOrEmpty(search[SearchCategory.Name]))
            {
                restaurants = restaurants.RestaurantsBySearchName(search[SearchCategory.Name].ToLower());
            }

            // modify the list when a user search a specified address
            if (!string.IsNullOrEmpty(search[SearchCategory.Address]))
            {
                restaurants = restaurants.RestaurantsBySearchAddress(search[SearchCategory.Address].ToLower());
            }

            // modify the list when a user search a specified score
            if (!string.IsNullOrEmpty(search[SearchCategory.Score]))
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
            // get the specified restaurant with his address and grade
            return await _context.Restaurants.Include(r => r.Address)
                                             .Include(r => r.Grade)
                                             .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Sets the positions of the restaurants.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Restaurant>> SetPositionsRestaurants()
        {
            // get the list of the restaurants with their address and grade
            var restaurants = await _context.Restaurants.Include(r => r.Address)
                                                                      .Include(r => r.Grade)
                                                                      .ToListAsync();
            
            // order the list
            restaurants = restaurants.OrderByDescendingRestaurants();

            // modify the position of all restaurants
            for (int i = 0; i < restaurants.Count; i++)
            {
                restaurants[i].Position = i + 1;
            }

            // update the list of restaurants and return it
            return await UpdateList(restaurants);
        }

        /// <summary>
        /// Updates the list of restaurants.
        /// </summary>
        /// <param name="restaurants">The restaurants.</param>
        /// <returns></returns>
        public async Task<List<Restaurant>> UpdateList(List<Restaurant> restaurants)
        {
            // modify each restaurant
            foreach (var resto in restaurants)
            {
                _context.Entry(resto).State = EntityState.Modified;
            }

            // apply in database
            await _context.SaveChangesAsync();

            // return list
            return restaurants;
        }

        /// <summary>
        /// Gets the best restaurants.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public async Task<List<Restaurant>> GetBestRestaurants(int? number)
        {
            // return the list of restaurants ordering by position
            var restaurants = await SetPositionsRestaurants();

            // if we want to take x best restaurants
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
            // add the restaurant to the database
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        /// <summary>
        /// Add multiple restaurants
        /// </summary>
        /// <param name="restaurants">The list of restaurants</param>
        /// <returns></returns>
        public async Task<List<Restaurant>> AddMultiple(List<Restaurant> restaurants)
        {
            // add a list of restaurants in the database
            _context.Restaurants.AddRange(restaurants);
            await _context.SaveChangesAsync();

            return restaurants;
        }
        
        /// <summary>
        /// Deletes the specified restaurant.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Restaurant> Delete(int id)
        {
            // find the restaurant with the specified id
            var restaurant = await _context.Restaurants.FindAsync(id);

            // remove it in the database
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
            // update the specified restaurant in the database
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
            // check if there is at least one restaurant with the specified id
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
