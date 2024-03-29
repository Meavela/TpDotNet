﻿using System;
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
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RestaurantContextTests
    {
        List<Restaurant> result = JsonSerializer.Deserialize<List<Restaurant>>(new RestaurantJson().ReadData(@".\Resources\restaurants.net.json"));

        [SetUp]
        public void Setup()
        {

            DeleteTables();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteTables();

            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
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
        /// Initializes the database connection.
        /// </summary>
        [Test]
        public void InitDbConnection()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.ToList();
            }
        }

        /// <summary>
        /// Tests the insert resto in database.
        /// </summary>
        [Test]
        public void TestInsertRestoInDb()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                for (int i = 0; i < 10; i++)
                {
                    var resto = new Restaurant
                    {
                        Name = "Mon resto " + i,
                        Comment = "Test",
                        MailOwner = "test@gmail.com",
                        Phone = "00.00.00.00.00"
                    };
                    dbContext.Restaurants.Add(resto);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Tests the insert address in database.
        /// </summary>
        [Test]
        public void TestInsertAddressInDb()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                for (int i = 0; i < 10; i++)
                {
                    var resto = new Restaurant
                    {
                        Name = "Mon resto " + i,
                        Comment = "Test",
                        MailOwner = "test@gmail.com",
                        Phone = "00.00.00.00.00"
                    };

                    var address = new Address
                    {
                        City = "Grenoble",
                        Street = "Test",
                        ZipCode = "38000",
                        RestaurantId = i
                    };
                    resto.Address = address;
                    dbContext.Restaurants.Add(resto);
                    dbContext.Addresses.Add(address);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Tests the insert grade in database.
        /// </summary>
        [Test]
        public void TestInsertGradeInDb()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                for (int i = 0; i < 10; i++)
                {
                    var resto = new Restaurant
                    {
                        Name = "Mon resto " + i,
                        Comment = "Test",
                        MailOwner = "test@gmail.com",
                        Phone = "00.00.00.00.00"
                    };

                    var grade = new Grade
                    {
                        Score = 8,
                        Comment = "Test",
                        RestaurantId = i
                    };

                    resto.Grade = grade;

                    dbContext.Restaurants.Add(resto);
                    dbContext.Grades.Add(grade);
                }

                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Create 10 restaurants.
        /// </summary>
        [Test]
        public void Create10Restaurants()
        {
            using (var dbContext = new RestaurantContext())
            {
                dbContext.Database.EnsureCreated();
                dbContext.Restaurants.AddRange(result);
                dbContext.SaveChanges();
            }
        }
    }
}
