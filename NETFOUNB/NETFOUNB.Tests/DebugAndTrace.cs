using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using NLog;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class DebugAndTrace
    {
        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine("hoi");
        }

        [TestMethod]
        public void MyTestMethod()
        {
            Debug.Assert(true);
        }

        [TestMethod]
        public void WatIsEenDebugWriteIf()
        {
            DebugWrite();

            Debug.WriteIf(DateTime.Today.DayOfWeek == DayOfWeek.Thursday,
                "Deze info is alleen nuttig op donderdag.");
            Debug.WriteIf(DateTime.Today.DayOfWeek != DayOfWeek.Thursday,
                "Deze info is alleen nuttig op de overige dagen van de week.");
        }

        [Conditional("ONZIN")]
        private static void DebugWrite()
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Thursday)
            {
                Debug.Write("Deze info is alleen nuttig op donderdag");
            }
            else
            {
                Debug.Write("Deze info is alleen nuttig op de overige dagen van de week.");
            }
        }

        /// <summary>
        /// Eenmalig uitvoeren in een *elevated* PowerShell:
        ///   New-EventLog -LogName Application -Source "demo"
        /// </summary>
        [TestMethod]
        public void NLogDemo()
        {
            var log = LogManager.GetCurrentClassLogger();
            log.Info("hoi");
        }
    }
}
