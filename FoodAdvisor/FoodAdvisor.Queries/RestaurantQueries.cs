using System.Collections.Generic;
using System.Linq;
using FoodAdvisor.Models;

namespace FoodAdvisor.Queries
{
    public static class RestaurantQueries
    {
        public static List<Restaurant> BestRestaurants(this List<Restaurant> input, int? number = null)
        {
            var restaurants = input.OrderByDescending(x => x.Grade.Score).ToList();

            if (number != null)
            {
               var list = restaurants.Take((int)number);
               return list.ToList();
            }

            return restaurants;
        }
    }
}
