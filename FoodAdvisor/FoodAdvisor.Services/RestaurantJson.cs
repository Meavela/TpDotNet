using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
            return File.ReadAllText(path, Encoding.GetEncoding("iso-8859-1"));
        }

        /// <summary>
        /// Imports the specified restaurants.
        /// </summary>
        /// <param name="path">The path.</param>
        public async Task<bool> Import(string path)
        {
            try
            {
                RestaurantServices services = new RestaurantServices();
                var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(ReadData(path));
                await services.AddMultiple(restaurants);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Writes in the file.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="path">The path.</param>
        public bool WriteFile(IEnumerable<Restaurant> data, string path)
        {
            try
            {
                var jsonContent = JsonSerializer.Serialize(data);
                File.WriteAllText(path, jsonContent);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
