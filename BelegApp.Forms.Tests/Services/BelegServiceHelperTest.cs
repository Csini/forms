using BelegApp.Forms.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BelegApp.Forms.UWP.Utils;
using BelegApp.Forms.Models;
using System;
using BelegApp.Forms.Services;

namespace BelegApp.Forms.Tests.Services
{
    [TestClass]
    public class BelegServiceHelperTest
    {
        Storage database;
        BelegServiceHelper helper;
         
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

            helper = new BelegServiceHelper(database);
        }

        [TestMethod]
        public void NoContentShouldChangeNothing()
        {
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[0], new Beleg[0]).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(0, result.Length, result.ToString());
        }

        private static Beleg createBeleg(Beleg.StatusEnum status)
        {
            return new Beleg(1, "Backend", DateTime.UtcNow.Date, "Test", 1, status, null, null);
        }

        [TestMethod]
        public void OnlyBackendShouldInsert()
        {
            Beleg beleg = createBeleg(Beleg.StatusEnum.ERFASST);
            Assert.AreEqual(1, helper.DoRefreshStatus(new Beleg[] { beleg }, new Beleg[0]).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(beleg.Status, result[0].Status);
        }

        [TestMethod]
        public void OnlyBackendGebuchtShouldNotInsert()
        {
            Beleg beleg = createBeleg(Beleg.StatusEnum.GEBUCHT);
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[] { beleg }, new Beleg[0]).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(0, result.Length, result.ToString());
        }

        [TestMethod]
        public void OnlyBackendAbgelehntShouldNotInsert()
        {
            Beleg beleg = createBeleg(Beleg.StatusEnum.ABGELEHNT);
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[] { beleg }, new Beleg[0]).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(0, result.Length, result.ToString());
        }

        [TestMethod]
        public void OnlyLocalShouldChangeNothing()
        {
            Beleg beleg = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(beleg).Wait();
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[0], new Beleg[] { beleg }).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(beleg.Status, result[0].Status);
        }

        [TestMethod]
        public void MatchedShouldChange()
        {
            Beleg backend = createBeleg(Beleg.StatusEnum.EXPORTIERT);
            Beleg local = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(local).Wait();
            Assert.AreEqual(1, helper.DoRefreshStatus(new Beleg[] { backend }, new Beleg[] { local }).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(backend.Status, result[0].Status);
        }

        [TestMethod]
        public void MatchedGebuchtShouldChange()
        {
            Beleg backend = createBeleg(Beleg.StatusEnum.GEBUCHT);
            Beleg local = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(local).Wait();
            Assert.AreEqual(1, helper.DoRefreshStatus(new Beleg[] { backend }, new Beleg[] { local }).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(backend.Status, result[0].Status);
        }

        [TestMethod]
        public void MatchedAbgelehntShouldChange()
        {
            Beleg backend = createBeleg(Beleg.StatusEnum.ABGELEHNT);
            Beleg local = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(local).Wait();
            Assert.AreEqual(1, helper.DoRefreshStatus(new Beleg[] { backend }, new Beleg[] { local }).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(backend.Status, result[0].Status);
        }

        // Backend not testable! [TestMethod]
        public void UnmatchedShouldInsert()
        {
            Beleg backend = createBeleg(Beleg.StatusEnum.EXPORTIERT);
            backend.Belegnummer = 2;
            Beleg local = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(local).Wait();
            Assert.AreEqual(1, helper.DoRefreshStatus(new Beleg[] { backend }, new Beleg[] { local }).Result, "DoRefreshStatus");
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(2, result.Length, result.ToString());
            Assert.AreEqual(local.Status, result[0].Status);
            Assert.AreEqual(backend.Status, result[1].Status);
        }

        [TestMethod]
        public void MultipleMatchedShouldChangeAll()
        {
            Beleg backend1 = createBeleg(Beleg.StatusEnum.ABGELEHNT);
            Beleg local1 = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(local1).Wait();
            Beleg backend2 = createBeleg(Beleg.StatusEnum.GEBUCHT);
            Beleg local2 = createBeleg(Beleg.StatusEnum.ERFASST);
            backend2.Belegnummer = 2;
            local2.Belegnummer = 2;
            Assert.AreEqual(2, helper.DoRefreshStatus(new Beleg[] { backend1, backend2 }, new Beleg[] { local1, local2 }).Result);
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(2, result.Length, result.ToString());
            Assert.AreEqual(backend1.Status, result[0].Status);
            Assert.AreEqual(backend2.Status, result[1].Status);
        }
    }
}

