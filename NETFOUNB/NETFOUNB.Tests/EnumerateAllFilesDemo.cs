using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class EnumerateAllFilesDemo
    {
        [TestMethod]
        public void SkipDirectoriesOnException()
        {
            EnumerateDirectories(@"C:\", CleanDirectory);
        }

        private void CleanDirectory(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path, "*.suo"))
            {
                File.Delete(file);
            }
        }

        private static void EnumerateDirectories(string path, Action<string> action)
        {
            action(path);

            foreach (var item in Directory.EnumerateDirectories(path))
            {
                try
                {
                    EnumerateDirectories(item, action);
                }
                catch (UnauthorizedAccessException)
                {
                    Debug.WriteLine($"Failed to access {item}");
                }
            }
        }
    }
}
