using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using FoodAdvisor.Models;

namespace FoodAdvisor.Services
{
    public class RestaurantJson
    {

        /// <summary>
        /// Loads the data of the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>List of restaurants</returns>
        public string ReadData(string path)
        {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// Imports the specified restaurants.
        /// </summary>
        /// <param name="path">The path.</param>
        public async void Import(string path)
        {
            RestaurantServices services = new RestaurantServices();
            var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(ReadData(path));
            foreach (var resto in restaurants)
            {
                await services.Add(resto);
            }
        }

        /// <summary>
        /// Writes in the file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        public void WriteFile(IEnumerable<Restaurant> data, string path)
        {
            var jsonContent = JsonSerializer.Serialize(data);
            File.WriteAllText(path, jsonContent);
        }
    }
}
