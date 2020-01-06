using System;
using FoodAdvisor.Models;
using FoodAdvisor.Services;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using NUnit.Framework;

namespace FoodAdvisor.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RestaurantJsonTests
    {
        List<Restaurant> result = JsonSerializer.Deserialize<List<Restaurant>>(new RestaurantJson().ReadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\restaurants.net.json"));

        [SetUp]
        public void Setup()
        {
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
        public void TestReadData()
        {
            Assert.AreEqual(10, result.Count, "Le fichier n'est pas correctement chargé");
        }

        [Test]
        public void TestWrite()
        {
            var filtre = result.Where(r => r.Address.City == "Grenoble");
            new RestaurantJson().WriteFile(filtre, @"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\testFile.net.json");
            var result2 = JsonSerializer.Deserialize<List<Restaurant>>(new RestaurantJson().ReadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\testFile.net.json"));
            Assert.AreEqual(5, result2.Count, "Le fichier n'a pas été correctement enregistré");
        }

        [Test]
        public void TestImport()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
            }
            new RestaurantJson().Import(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\restaurants.net.json");
            var restaurants = new RestaurantServices().GetAll();

            Assert.AreEqual(10, restaurants.Result.Count, "Le fichier n'est pas correctement chargé");
        }
    }
}
