using BelegApp.Forms.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BelegApp.Forms.UWP.Utils;
using BelegApp.Forms.Models;
using System;

namespace BelegApp.Forms.Tests.Utils
{
    [TestClass]
    public class StorageTest
    {

        Storage database;
         
        [TestInitialize]
        public void init()
        {
            DependencyServiceMock dependencyService = new DependencyServiceMock();
            // Use the testable stub for unit tests
            dependencyService.Register<IFileHelper>(new FileHelper());

            database = new Storage(dependencyService);

            foreach (Beleg beleg in database.GetBelege().Result)
            {
                database.RemoveBeleg(beleg).Wait();
            }
            Assert.AreEqual(0, database.GetBelege().Result.Length);
        }

        [TestMethod]
        public void InitialListShouldBeEmpty()
        {
            Assert.AreEqual(0, database.GetBelege().Result.Length);
        }

        [TestMethod]
        public void InitialStoreShouldResultInOneElementList()
        {
            Beleg beleg = new Beleg(null, "Testbeleg", null, DateTime.UtcNow.Date, "Test", 100, Beleg.StatusEnum.ERFASST, null, null);
            int nr = database.StoreBeleg(beleg).Result;
            Models.Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
        }

        [TestMethod]
        public void InitialStoreShouldResultInBeleg()
        {
            Beleg beleg = new Beleg(null, "Testbeleg", null, DateTime.UtcNow.Date, "Test", 100, Beleg.StatusEnum.ERFASST, null, null);
            database.StoreBeleg(beleg).Wait();
            Assert.IsNotNull(database.GetBeleg(beleg.Belegnummer.Value).Result);
        }

        [TestMethod]
        public void DeleteAfterStoreShouldBeEmpty()
        {
            Beleg beleg = new Beleg(null, "Testbeleg", null, DateTime.UtcNow.Date, "Test", 100, Beleg.StatusEnum.ERFASST, null, null);
            database.StoreBeleg(beleg).Wait();
            Models.Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            database.RemoveBeleg(result[0]).Wait();
            Assert.AreEqual(0, database.GetBelege().Result.Length, result.ToString());
        }

        [TestMethod]
        public void ConsecutiveStoreShouldChangeBeleg()
        {
            Beleg beleg = new Beleg(null, "Testing", "Testbeleg", DateTime.UtcNow.Date, "Test", 100, Beleg.StatusEnum.ERFASST, null, null);
            database.StoreBeleg(beleg).Wait();
            beleg = database.GetBeleg(beleg.Belegnummer.Value).Result;
            Assert.AreEqual("Testbeleg", beleg.Description);
            beleg.Description = "Changed";
            database.StoreBeleg(beleg).Wait();
            Assert.AreEqual("Changed", database.GetBeleg(beleg.Belegnummer.Value).Result.Description);
        }
    }
}
