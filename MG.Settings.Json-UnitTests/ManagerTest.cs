using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Runtime.Serialization;

[assembly: Parallelize(Scope = ExecutionScope.MethodLevel, Workers = 0)]
namespace MG.Settings.Json.Tests
{
    [TestClass]
    public class ManagerTest
    {
        private const string COMPANY_NAME = "ABC Medical";
        private const string CUSTOMER_NAME = "Acme Packaging Corporation";

        private static uint _testCompanyId;
        private static Guid _testCompanyUid;
        private static Guid _testCustomerUid;
        private static string tempPath;
        public static MySettingManager manager;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
            _testCompanyId = GetRandomNumber();
            _testCompanyUid = Guid.NewGuid();
            _testCustomerUid = Guid.NewGuid();
            WriteToTempFile(tempPath);

            manager = new MySettingManager
            {
                FilePath = tempPath
            };
            manager.Read();
        }

        private static uint GetRandomNumber()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] rBytes = new byte[4];
                rng.GetBytes(rBytes);
                return BitConverter.ToUInt32(rBytes, 0);
            }
        }

        [TestMethod]
        public void TestCompanyName() => Assert.AreEqual(COMPANY_NAME, manager.Company);

        [TestMethod]
        public void TestCompanyNumber() => Assert.AreEqual(_testCompanyId, manager.CompanyNumber);

        [TestMethod]
        public void TestCompanyId() => Assert.AreEqual(_testCompanyUid, manager.CompanyId);

        [TestMethod]
        public void TestCustomers()
        {
            Assert.IsNotNull(manager.Customers);
            Assert.IsTrue(manager.Customers.Count == 1);
        }

        [TestMethod]
        public void TestCustomerInfo()
        {
            Assert.AreEqual("bob@whatev.info", manager.Customers[0].ContactInfo);
            Assert.AreEqual(_testCustomerUid, manager.Customers[0].Id);
            Assert.AreEqual(CUSTOMER_NAME, manager.Customers[0].Name);
        }

        private static void WriteToTempFile(string filePath)
        {
            var job = new JObject
            {
                new JProperty("company", COMPANY_NAME),
                new JProperty("companyId", _testCompanyId),
                new JProperty("companyUniqueId", _testCompanyUid),
                new JProperty("customers", new JArray
                {
                    new JObject
                    {
                        new JProperty("customerContactInfo", "bob@whatev.info"),
                        new JProperty("customerId", _testCustomerUid),
                        new JProperty("customerName", CUSTOMER_NAME)
                    }
                })
            };
            using (var sw = new StreamWriter(filePath))
            {
                using (var jtw = new JsonTextWriter(sw)
                {
                    AutoCompleteOnClose = true,
                    CloseOutput = true,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    Formatting = Formatting.Indented,
                    Indentation = 1,
                    IndentChar = char.Parse("\t")
                })
                {
                    job.WriteTo(jtw);
                }
            }
        }

        [ClassCleanup]
        public static void Cleanup() => File.Delete(tempPath);
    }
}
