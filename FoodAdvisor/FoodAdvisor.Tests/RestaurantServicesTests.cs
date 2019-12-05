using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
            result = new RestaurantJson().LoadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\restaurants.net.json");
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
        public void TestGetAll()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();

                RestaurantServices services = new RestaurantServices(dbContext);
                var restaurants = services.GetAll().Result;

                Assert.IsTrue(restaurants.Any(r => r.Name == "Au Dragon d'Or"));
            }
        }

        [Test]
        public void TestGetOne()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();

                RestaurantServices services = new RestaurantServices(dbContext);
                var restaurant = services.Get(1).Result;

                Assert.IsTrue(restaurant.Name == "Au Dragon d'Or");
            }
        }

        [Test]
        public void TestAdd()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            using (var dbContext = new RestaurantContext())
            {
                var resto = new Restaurant
                {
                    Name = "Test"
                };

                RestaurantServices services = new RestaurantServices(dbContext);
                var newResto = services.Add(resto).Result;
                var getResto = services.Get(11).Result;

                Assert.IsTrue(newResto == getResto);
            }
        }

        [Test]
        public void TestUpdate()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            using (var dbContext = new RestaurantContext())
            {
                var resto = new Restaurant
                {
                    Id = 1,
                    Name = "Test"
                };

                RestaurantServices services = new RestaurantServices(dbContext);
                var updateResto = services.Update(resto).Result;
                var getResto = services.Get(1).Result;

                Assert.IsTrue(updateResto == getResto);
            }
        }

        [Test]
        public void TestDelete()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }

            using (var dbContext = new RestaurantContext())
            {
                RestaurantServices services = new RestaurantServices(dbContext);
                var deleteResto = services.Delete(1).Result;
                var getResto = services.Get(1).Result;

                Assert.IsTrue(getResto == null);
            }
        }

    }
}
