using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App.SousTypes.Models;
using NUnit.Framework;

namespace App.SousTypes.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class JsonAnimalTest
    {
        [Test]
        public void TestWrite()
        {
            var animals = new List<Animal>
            {
                new Cat {Name = "Felix", DoMiaou = "Miaou"},
                new Dog {Name = "Medor", Breed = "Caniche Nain"}
            };

            var writeResult = new JsonRepo<Animal>().Write(animals);
            var readResult = new JsonRepo<Animal>().Read(writeResult);

            Assert.IsTrue(readResult.Any(a => a.Type == "Dog"));
        }
    }
}