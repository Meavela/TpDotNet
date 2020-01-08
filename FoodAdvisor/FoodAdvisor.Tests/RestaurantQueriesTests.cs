using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using FoodAdvisor.Models;
using FoodAdvisor.Queries;
using FoodAdvisor.Services;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace FoodAdvisor.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class RestaurantQueriesTests
    {
        public List<Restaurant> result;

        [SetUp]
        public void Setup()
        {
            result = JsonSerializer.Deserialize<List<Restaurant>>(new RestaurantJson().ReadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\restaurants.net.json"));
            using (SqlConnection connection = new SqlConnection(@"server=Poulpe;database=FoodAdvisor;trusted_connection=true;"))
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
            var restos = services.SetPositionsRestaurants().Result.BestRestaurants(5);

            Assert.IsTrue(restos.Count == 5);
        }

        /// <summary>
        /// Tests to get restaurants order by position.
        /// </summary>
        [Test]
        public void TestOrderByPosition()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var restos = services.SetPositionsRestaurants().Result.OrderByPositionRestaurants();

            Assert.IsTrue(restos[0].Position == 1);
            Assert.IsTrue(restos[1].Position == 2);
        }

        /// <summary>
        /// Tests to get restaurants by search the name.
        /// </summary>
        [Test]
        public void TestRestaurantBySearchName()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }
            
            RestaurantServices services = new RestaurantServices();
            var restaurants = services.GetAll().Result.RestaurantsBySearchName("sushi licious");

            Assert.IsTrue(restaurants.Count == 1);
        }

        /// <summary>
        /// Tests to get restaurants by search address.
        /// </summary>
        [Test]
        public void TestRestaurantBySearchAddress()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var restaurants = services.GetAll().Result.RestaurantsBySearchAddress("grenoble");

            Assert.IsTrue(restaurants.Count == 5);
        }

        /// <summary>
        /// Tests to get restaurants by search score.
        /// </summary>
        [Test]
        public void TestRestaurantBySearchScore()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var restaurants = services.GetAll().Result.RestaurantsBySearchScore("5");

            Assert.IsTrue(restaurants.Count == 1);
        }

        /// <summary>
        /// Tests to get restaurants order by descending score.
        /// </summary>
        [Test]
        public void TestOrderByDescendingRestaurants()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            RestaurantServices services = new RestaurantServices();
            var restos = services.GetAll().Result.OrderByDescendingRestaurants();

            Assert.IsTrue(result.Count == restos.Count);
            Assert.IsTrue(restos[0].Grade.Score >= restos[1].Grade.Score);
        }
    }
}