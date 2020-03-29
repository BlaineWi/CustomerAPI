using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CustomerAPI3.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestDataAccess 
    {
        #region "Boilerplate"

        private static TestContext Context;

        [ClassInitialize]
        public static void InitClass(TestContext testContext)
        {
            Context = testContext;
        }

        #endregion

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("DataFactory")]
        public void TestDataFactory_PersonList()
        {
            var pl = DataAccess.DataFactory.PersonList;
            Assert.IsNotNull(pl);
            Assert.IsTrue(pl.Count > 0);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("DataFactory")]
        public void TestDataFactory_Queryable()
        {
            var pl = DataAccess.DataFactory.People;
            Assert.IsNotNull(pl);
            var p = pl.FirstOrDefault();
            Assert.IsNotNull(p);
            var q = pl.Where(p => p.NameLast.Contains("t")).FirstOrDefault();
            Assert.IsNotNull(q);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("DataFactory")]
        public void TestDataFactory_IsEmpty()
        {
            var entity = new Models.Address();
            Assert.IsTrue(entity.IsEmpty);
        }

    }
}
