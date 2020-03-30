using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BlitzkriegSoftware.MsTest;
using System.Diagnostics.CodeAnalysis;

namespace CustomerAPI3.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestModels 
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
        [TestCategory("Serialize")]
        public void TestSerialize_BlitzAssemblyVersionMetadata()
        {
            var bav = new Models.BlitzAssemblyVersionMetadata()
            {
                Company = "company",
                Copyright = "copyright",
                Description = "description",
                FileVersion = "3.2.1",
                Product = "product",
                SemanticVersion = "3.2.1"
            };

            TestJsonSerializationHelper.AssertJsonSerialization<Models.BlitzAssemblyVersionMetadata>(TestModels.Context, bav);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Serialize")]
        public void TestSerialize_Customer()
        {

            var a1 = new Models.Address()
            {
                Address1 = "101 Maple St",
                Address2 = "Suite 3",
                City = "AnyTown",
                Kind = Models.AddressKind.Mailing,
                State = "CA",
                Zip = "98765"
            };

            var a2 = new Models.Address()
            {
                Address1 = "131 Ash Ave",
                Address2 = "Suite 7b",
                City = "AllCity",
                Kind = Models.AddressKind.Billing,
                State = "CA",
                Zip = "93456"
            };

            var p = new Dictionary<string, string>()
            {
                { "A", "a" },
                { "B", "b" }
            };

            var bav = new Models.Customer()
            {
                Birthday = new DateTime(1962, 5, 2),
                Company = "My Co",
                EMail = "yo@there.org",
                NameFirst = "Janet",
                NameLast = "Squizzle",
                _id = Guid.NewGuid().ToString()
            };

            foreach (var d in p)
            {
                bav.Preference.Add(d.Key, d.Value);
            }

            bav.Addresses.Add(a1);
            bav.Addresses.Add(a2);

            TestJsonSerializationHelper.AssertJsonSerialization<Models.Customer>(TestModels.Context, bav);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        [TestCategory("Serialize")]
        public void TestSerialize_ErrorPayload()
        {
            var d = new Dictionary<string, string>() {
                {  "A", "a"},
                {  "B", "b"}
            };
            
            var ep = new Models.ErrorPayload()
            {
                Message = "message",
                StackTrace = "trace",
                StatusCode = 418,
            };

            foreach (var e in d) ep.Data.Add(e.Key, e.Value);

            TestJsonSerializationHelper.AssertJsonSerialization<Models.ErrorPayload>(TestModels.Context, ep);
        }

    }
    }
