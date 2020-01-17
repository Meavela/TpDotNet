using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using FoodAdvisor.Models;
using FoodAdvisor.Services;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace FoodAdvisor.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RestaurantServicesTests
    {
        public List<Restaurant> result;

        [SetUp]
        public void Setup()
        {
            result = JsonSerializer.Deserialize<List<Restaurant>>(new RestaurantJson().ReadData(@".\Resources\restaurants.net.json"));

            DeleteTables();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteTables();

            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                //dbContext.Restaurants.AddRange(result);
                //dbContext.SaveChanges();
            }
        }

        public void DeleteTables()
        {
            using (SqlConnection connection =
                new SqlConnection(@"server=Poulpe;database=FoodAdvisor;trusted_connection=true;"))
            {
                string sql = @"if (exists(Select 1 from sys.tables where name = 'Grades'))
                                DROP Table Grades
                                if (exists(Select 1 from sys.tables where name = 'Addresses'))
                                DROP Table Addresses
                                if (exists(Select 1 from sys.tables where name = 'Restaurants'))
                                DROP Table Restaurants";

                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// Tests to get all restaurants.
        /// </summary>
        [Test]
        public void TestGetAll()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();

                RestaurantServices services = new RestaurantServices();
                var restaurants = services.GetAll().Result;

                Assert.IsTrue(restaurants.Any(r => r.Name == "Au Dragon d'Or"), "Ne récupère pas tous les restaurants");
            }
        }

        /// <summary>
        /// Tests to get one restaurant.
        /// </summary>
        [Test]
        public void TestGetOne()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();

                RestaurantServices services = new RestaurantServices();
                var restaurant = services.Get(1).Result;

                Assert.IsTrue(restaurant.Name == "Au Dragon d'Or", "Ne récupère pas le restaurant correspondant");
            }
        }

        /// <summary>
        /// Tests to add a restaurant.
        /// </summary>
        [Test]
        public void TestAdd()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            var resto = new Restaurant
            {
                Name = "Test",
                Comment = "Test",
                MailOwner = "test@gmail.com",
                Phone = "00.00.00.00.00"
            };

            RestaurantServices services = new RestaurantServices();
            var newResto = services.Add(resto).Result;
            var getResto = services.Get(11).Result;

            Assert.IsTrue(newResto == getResto, "N'ajoute pas en base de données le restaurant");
        }

        /// <summary>
        /// Tests to update a restaurant.
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            var resto = new Restaurant
            {
                Id = 1,
                Name = "Test",
                Comment = "Test",
                MailOwner = "test@gmail.com",
                Phone = "00.00.00.00.00"
            };

            RestaurantServices services = new RestaurantServices();
            var updateResto = services.Update(resto).Result;
            var getResto = services.Get(1).Result;

            Assert.IsTrue(updateResto == getResto, "Ne change pas le restaurant en base de données");
        }

        /// <summary>
        /// Tests to delete a restaurant.
        /// </summary>
        [Test]
        public void TestDelete()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var deleteResto = services.Delete(1).Result;
            var getResto = services.Get(1).Result;

            Assert.IsTrue(getResto == null, "Ne supprime pas le restaurant de la base de données");
        }

        /// <summary>
        /// Tests if a restaurant exists.
        /// </summary>
        [Test]
        public void TestIsExist()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            Assert.IsTrue(services.IsExists(1), "Le restaurant n'existe pas en base de données");
        }

        /// <summary>
        /// Tests to get best restaurants.
        /// </summary>
        [Test]
        public void TestBestRestaurants()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var restos = services.GetBestRestaurants(null).Result;

            Assert.IsTrue(restos.Count == result.Count, "Le nombre de restaurants ne correspond pas");
            Assert.IsTrue(restos[0].Grade.Score >= restos[1].Grade.Score, "Ne trie pas correctement les restaurants");
        }

        /// <summary>
        /// Tests to set positions of restaurants.
        /// </summary>
        [Test]
        public void TestSetPositions()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();

            var restaurants = services.SetPositionsRestaurants().Result;

            foreach (var resto in restaurants)
            {
                Assert.IsTrue(resto.Position != null, "Ne change pas les positions des restaurants");
            }
        }

        /// <summary>
        /// Tests to get restaurants by search.
        /// </summary>
        [Test]
        public void TestGetBySearch()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            Dictionary<SearchCategory,string> search = new Dictionary<SearchCategory, string>();
            search.Add(SearchCategory.Name, "Restaurant");
            search.Add(SearchCategory.Address, "Lyon");
            search.Add(SearchCategory.Score, "10");
            var restaurants = services.GetBySearch(search).Result;

            Assert.IsTrue(restaurants.Count == 1, "N'a pas trouvé le bon nombre de restaurant correspondant");
            Assert.IsTrue(restaurants[0].Name.Contains("Restaurant"), "Le restaurant ne contient pas 'Restaurant' dans son nom");
            Assert.IsTrue(restaurants[0].Address.City == "Lyon", "Le restaurant ne contient pas 'Lyon' dans son adresse");
            Assert.IsTrue(restaurants[0].Grade.Score == 10, "Le restaurant n'a pas un score de 10");
        }

        /// <summary>
        /// Tests to get 5 best restaurants.
        /// </summary>
        [Test]
        public void TestBest5Restaurants()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var restos = services.GetBestRestaurants(5).Result;

            Assert.IsTrue(restos.Count == 5, "Ne récupère pas 5 restaurants");
            Assert.IsTrue(restos[0].Grade.Score >= restos[1].Grade.Score, "Ne trie pas corretement les 5 restaurants");
        }

    }
}
