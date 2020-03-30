using CustomerAPI3.Libs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace CustomerAPI3.Test
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestLibs
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
        [TestCategory("Functional")]
        public void TestLibs_AssembyInfoHelper()
        {
            var assembly = typeof(Program).Assembly;
            foreach (var attribute in assembly.GetCustomAttributesData())
            {
                if (!attribute.TryParse(out string value))
                {
                    value = string.Empty;
                }
                var name = attribute.AttributeType.Name;
                Context.WriteLine($"{name}, {value}");
                Assert.IsFalse(string.IsNullOrWhiteSpace(name));
            }
        }

    }
}
