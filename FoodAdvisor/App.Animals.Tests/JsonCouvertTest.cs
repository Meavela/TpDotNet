using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using App.SousTypes.Models.Others;
using NUnit.Framework;

namespace App.SousTypes.Tests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class JsonCouvertTest
    {
        [Test]
        public void TestWrite()
        {
            var couverts = new List<Couvert>
            {
                new Couteau {Name = "MonCouteau", AvecDents = true},
                new Cuillere {Name = "MaCuillere", TypeDeCuillere = "Grande"},
                new Fourchette {Name = "MaFourchette", NombreDeDents = 3}
            };

            var writeResult = new JsonRepo<Couvert>().Write(couverts);
            var readResult = new JsonRepo<Couvert>().Read(writeResult);

            Assert.IsTrue(readResult.Any(a => a.Type == "Fourchette"));
        }
    }
}