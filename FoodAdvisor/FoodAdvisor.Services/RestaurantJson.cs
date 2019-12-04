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
        public List<Restaurant> LoadData(string path)
        {
            var fileContent = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Restaurant>>(fileContent);
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
