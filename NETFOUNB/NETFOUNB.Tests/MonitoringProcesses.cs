using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class MonitoringProcesses
    {
        [TestMethod]
        public void TestMethod1()
        {
            var all = Process.GetProcesses();
            Console.WriteLine(string.Join(", ", all.Select(p => p.ProcessName)));
        }

        [TestMethod]
        public void KillProcess()
        {
            Process
                .GetProcessesByName("chrome")
                .ToList()
                .ForEach(p => p.Kill());
        }

        [TestMethod]
        public void StartProcess()
        {
            var path = Guid.NewGuid().ToString();
            path = Path.ChangeExtension(path, "txt");

            string original = "Dit moet je aanpassen.";
            File.WriteAllText(path, original);

            var p = Process.Start("notepad", path);
            p.WaitForExit();

            var text = File.ReadAllText(path);
            Assert.AreNotEqual(text, original, "De tekst is niet aangepast");
        }
    }
}
