using FoodAdvisor.Models;
using FoodAdvisor.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NUnit.Framework;

namespace FoodAdvisor.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class RestaurantJsonTests
    {
        List<Restaurant> result = new RestaurantJson().LoadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\restaurants.net.json");

        [Test]
        public void TestLoadData()
        {
            Assert.AreEqual(10, result.Count, "Le fichier n'est pas correctement chargé");
        }

        [Test]
        public void TestWrite()
        {
            var filtre = result.Where(r => r.Address.City == "Grenoble");
            new RestaurantJson().WriteFile(filtre, @"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\testFile.net.json");
            var result2 = new RestaurantJson().LoadData(@"E:\Cours\B3\dotnet\TpDotNet\FoodAdvisor\FoodAdvisor.Tests\Resources\testFile.net.json");
            Assert.AreEqual(5, result2.Count, "Le fichier n'a pas été correctement enregistré");
        }
    }
}
