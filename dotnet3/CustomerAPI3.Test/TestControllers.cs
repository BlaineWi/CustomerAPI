using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CustomerAPI3.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestControllers 
    {

        #region "Boilerplate"

        private static TestContext Context;


        [ClassInitialize]
        public static void InitClass(TestContext testContext)
        {
            Context = testContext;
        }

        #endregion

        #region "Helpers"

        private BlitzkriegSoftware.SecureRandomLibrary.SecureRandom _dice;

        public BlitzkriegSoftware.SecureRandomLibrary.SecureRandom Dice
        {
            get
            {
                if (this._dice == null)
                {
                    this._dice = new BlitzkriegSoftware.SecureRandomLibrary.SecureRandom();
                }
                return this._dice;
            }
        }

        private List<string> _ids;

        public List<string> Ids
        {
            get
            {
                if (this._ids == null)
                {
                    var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
                    var ctlr = new Controllers.CustomerController(logger);
                    this._ids = new List<string>();
                    this._ids.AddRange(ctlr.IdList());
                }
                return this._ids;
            }
        }

        public void ResetIds()
        {
            this._ids = null;
            _ = this.Ids;
        }


        public string RandomId()
        {
            var index = this.Dice.Next(0, this.Ids.Count - 1);
            var id = this.Ids[index];
            return id;
        }

        public Models.Customer RandomCustomer()
        {
            var id = this.RandomId();
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            var entity = ctlr.Get(id);
            return entity;
        }


        #endregion

        #region "Common"

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Common_VersionGet()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CommonController>(TestControllers.Context);
            var ctlr = new Controllers.CommonController(logger);
            IActionResult result = ctlr.VersionGet();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Common_VersionInfo()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CommonController>(TestControllers.Context);
            var ctlr = new Controllers.CommonController(logger);
            IActionResult result = ctlr.VersionInfo();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Common_CheckHealth()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CommonController>(TestControllers.Context);
            var ctlr = new Controllers.CommonController(logger);
            var result = ctlr.CheckHealth();
            Assert.IsNotNull(result);
        }

        #endregion

        #region "Customer"

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Customer_Get()
        {
            var result = this.RandomCustomer();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        [ExpectedException(typeof(System.ArgumentException))]
        public void Controller_Customer_Get_Null()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            _ = ctlr.Get(null);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Controller_Customer_Get_NotFound()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            var id = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ";
            _ = ctlr.Get(id);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Customer_AddUpdate_Existing()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            Models.Customer entity = this.RandomCustomer();
            var result = ctlr.AddUpdate(entity);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Customer_AddUpdate_New()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            Models.Customer entity = this.RandomCustomer();
            entity._id = null;
            entity.NameLast = "Test";
            var result = ctlr.AddUpdate(entity);
            this.ResetIds();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Controller_Customer_AddUpdate_Bad()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            Models.Customer entity = null;
            var result = ctlr.AddUpdate(entity);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Customer_Delete_Ok()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            var id = this.RandomId();
            this.Ids.Remove(id);
            ctlr.Delete(id);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Controller_Customer_Delete_Bad()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            ctlr.Delete(null);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        public void Controller_Customer_Search()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            var entity = this.RandomCustomer();
            var text = entity.NameLast.Substring(0,2);
            var result = ctlr.Search(text);
            Assert.IsNotNull(result);
        }


        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Controller")]
        [ExpectedException(typeof(ArgumentException))]
        public void Controller_Customer_Search_Bad()
        {
            var logger = new BlitzkriegSoftware.MsTest.MsTestLogger<Controllers.CustomerController>(TestControllers.Context);
            var ctlr = new Controllers.CustomerController(logger);
            _ = ctlr.Search(null);
        }

        #endregion

    }
}
