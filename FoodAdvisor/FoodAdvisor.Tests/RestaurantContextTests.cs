//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics.CodeAnalysis;
//using System.Linq;
//using FoodAdvisor.Models;
//using FoodAdvisor.Services;
//using Microsoft.Data.SqlClient;
//using NUnit.Framework;

//namespace FoodAdvisor.Tests
//{
//    [TestFixture]
//    [ExcludeFromCodeCoverage]
//    public class RestaurantContextTests
//    {
//        List<Restaurant> result = new RestaurantJson().LoadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\restaurants.net.json");

//        [SetUp]
//        public void Setup()
//        {
//            using (SqlConnection connection = new SqlConnection(@"server=Poulpe;database=FoodAdvisor;trusted_connection=true;"))
//            {
//                string sql = @"if (exists(Select 1 from sys.tables where name = 'Grades'))
//                                DROP Table Grades
//                                if (exists(Select 1 from sys.tables where name = 'Addresses'))
//                                DROP Table Addresses
//                                if (exists(Select 1 from sys.tables where name = 'Restaurants'))
//                                DROP Table Restaurants";

//                try
//                {
//                    using (SqlCommand command = new SqlCommand(sql, connection))
//                    {
//                        command.CommandType = CommandType.Text;
//                        connection.Open();
//                        command.ExecuteNonQuery();
//                        connection.Close();
//                    }
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e);
//                    throw;
//                }
//            }
//        }

//        [Test]
//        public void InitDbConnection()
//        {
//            using (var dbContext = new RestaurantContext())
//            {
//                dbContext.Database.EnsureCreated();
//                dbContext.Restaurants.ToList();
//            }
//        }

//        [Test]
//        public void TestInsertRestoInDb()
//        {
//            using (var dbContext = new RestaurantContext())
//            {
//                dbContext.Database.EnsureCreated();
//                for (int i = 0; i < 10; i++)
//                {
//                    var resto = new Restaurant
//                    {
//                        Name = "Mon resto " + i
//                    };
//                    dbContext.Restaurants.Add(resto);
//                }

//                dbContext.SaveChanges();
//            }
//        }

//        //[Test]
//        public void TestInsertAddressInDb()
//        {
//            using (var dbContext = new RestaurantContext())
//            {
//                dbContext.Database.EnsureCreated();
//                for (int i = 0; i < 10; i++)
//                {
//                    var address = new Address
//                    {
//                        City = "Grenoble",
//                        RestaurantId = 5
//                    };
//                    dbContext.Addresses.Add(address);
//                }

//                dbContext.SaveChanges();
//            }
//        }

//        //[Test]
//        public void TestInsertGradeInDb()
//        {
//            using (var dbContext = new RestaurantContext())
//            {
//                dbContext.Database.EnsureCreated();
//                for (int i = 0; i < 10; i++)
//                {
//                    var grade = new Grade
//                    {
//                        Score = 8,
//                        RestaurantId = 5
//                    };
//                    dbContext.Grades.Add(grade);
//                }

//                dbContext.SaveChanges();
//            }
//        }

//        [Test]
//        public void Create10Restaurants()
//        {
//            using (var dbContext = new RestaurantContext())
//            {
//                dbContext.Database.EnsureCreated();
//                dbContext.Restaurants.AddRange(result);
//                dbContext.SaveChanges();
//            }
//        }
//    }
//}
