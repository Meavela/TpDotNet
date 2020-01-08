using System.Collections.Generic;
using System.Linq;
using FoodAdvisor.Models;

namespace FoodAdvisor.Queries
{
    public static class RestaurantQueries
    {
        public static List<Restaurant> OrderByPositionRestaurants(this List<Restaurant> input)
        {
            return input.OrderBy(x => x.Position).ToList();
        }

        public static List<Restaurant> OrderByDescendingRestaurants(this List<Restaurant> input)
        {
            return input.OrderByDescending(x => x.Grade.Score).ToList();
        }

        public static List<Restaurant> BestRestaurants(this List<Restaurant> input, int number)
        {
            return input.Where(x => x.Position <= 5).ToList();
        }

        public static List<Restaurant> RestaurantsBySearchName(this List<Restaurant> input, string search)
        {
            return input.Where(x => x.Name.ToLower().Contains(search)).ToList();
        }

        public static List<Restaurant> RestaurantsBySearchAddress(this List<Restaurant> input, string search)
        {
            return input.Where(x => x.Address.Street.ToLower().Contains(search) || x.Address.City.ToLower().Contains(search) || x.Address.ZipCode.ToLower().Contains(search)).ToList();
        }

        public static List<Restaurant> RestaurantsBySearchScore(this List<Restaurant> input, string search)
        {
            return input.Where(x => x.Grade.Score == int.Parse(search)).ToList();
        }
    }
}
