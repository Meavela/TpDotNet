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
            var restos = services.GetAll().Result;
            var bests = restos.BestRestaurants(5);

            Assert.IsTrue(bests.Count == 5);
            Assert.IsTrue(bests[0].Grade.Score >= bests[1].Grade.Score);

        }

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
            var restos = services.GetAll().Result;
            var bests = restos.BestRestaurants();

            Assert.IsTrue(bests.Count == restos.Count);
            Assert.IsTrue(bests[0].Grade.Score >= bests[1].Grade.Score);
        }
    }
}