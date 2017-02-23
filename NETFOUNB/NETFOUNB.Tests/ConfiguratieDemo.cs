using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class ConfiguratieDemo
    {
        [TestMethod]
        public void TestMethod1()
        {
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["cijferlijst"];
            Assert.AreEqual(connection.ConnectionString, "Server=.;Database=Cijfers;Trusted_Connection=True");
        }

        [TestMethod]
        public void AndereSettings()
        {
            var setting = System.Configuration.ConfigurationManager.AppSettings["demo"];
            Assert.AreEqual("iets", setting);
        }

        [TestMethod]
        public void MogenWeAlNaarHuis()
        {
            SettingsDemo.Default.MogenWeAlNaarHuis = true;
            SettingsDemo.Default.Save();

            Assert.IsTrue(SettingsDemo.Default.MogenWeAlNaarHuis);
        }
    }
}
