﻿using BelegApp.Forms.Utils;
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
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[0], new Beleg[0]));
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(0, result.Length, result.ToString());
        }

        private static Beleg createBeleg(Beleg.StatusEnum status)
        {
            return new Beleg(1, "Backend", DateTime.UtcNow.Date, "Test", 1, status, null, null);
        }

        [TestMethod]
        public void OnlyBackendShouldChangeNothing()
        {
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[] { createBeleg(Beleg.StatusEnum.ERFASST) }, new Beleg[0]));
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(0, result.Length, result.ToString());
        }

        [TestMethod]
        public void OnlyLocalShouldChangeNothing()
        {
            Beleg beleg = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(beleg).Wait();
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[0], new Beleg[] { beleg }));
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
            Assert.AreEqual(1, helper.DoRefreshStatus(new Beleg[] { backend }, new Beleg[] { local }));
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(backend.Status, result[0].Status);
        }

        [TestMethod]
        public void UnmatchedShouldChangeNothing()
        {
            Beleg backend = createBeleg(Beleg.StatusEnum.EXPORTIERT);
            backend.Belegnummer = 2;
            Beleg local = createBeleg(Beleg.StatusEnum.ERFASST);
            database.StoreBeleg(local).Wait();
            Assert.AreEqual(0, helper.DoRefreshStatus(new Beleg[] { backend }, new Beleg[] { local }), "DoRefreshStatus");
            Beleg[] result = database.GetBelege().Result;
            Assert.AreEqual(1, result.Length, result.ToString());
            Assert.AreEqual(local.Status, result[0].Status);
        }
    }
}

