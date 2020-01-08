using System.Collections.Generic;
using System.Linq;
using FoodAdvisor.Models;

namespace FoodAdvisor.Queries
{
    public static class RestaurantQueries
    {
        /// <summary>
        /// Orders the restaurants by position ascending.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static List<Restaurant> OrderByPositionRestaurants(this List<Restaurant> input)
        {
            return input.OrderBy(x => x.Position).ToList();
        }

        /// <summary>
        /// Orders the restaurants by score descending.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static List<Restaurant> OrderByDescendingRestaurants(this List<Restaurant> input)
        {
            return input.OrderByDescending(x => x.Grade.Score).ToList();
        }

        /// <summary>
        /// Get the Bests restaurants.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static List<Restaurant> BestRestaurants(this List<Restaurant> input, int number)
        {
            return input.Where(x => x.Position <= number).ToList();
        }

        /// <summary>
        /// Get the Restaurants by the name.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public static List<Restaurant> RestaurantsBySearchName(this List<Restaurant> input, string search)
        {
            return input.Where(x => x.Name.ToLower().Contains(search)).ToList();
        }

        /// <summary>
        /// Get the Restaurants by the address.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public static List<Restaurant> RestaurantsBySearchAddress(this List<Restaurant> input, string search)
        {
            return input.Where(x => x.Address.Street.ToLower().Contains(search) || x.Address.City.ToLower().Contains(search) || x.Address.ZipCode.ToLower().Contains(search)).ToList();
        }

        /// <summary>
        /// Get the Restaurants by the score.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="search">The search.</param>
        /// <returns></returns>
        public static List<Restaurant> RestaurantsBySearchScore(this List<Restaurant> input, string search)
        {
            return input.Where(x => x.Grade.Score == int.Parse(search)).ToList();
        }
    }
}
