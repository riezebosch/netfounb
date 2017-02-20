using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            File.WriteAllText("output.txt", "Goedemorgen allemaal.");
            string text = File.ReadAllText("output.txt");

            Assert.AreEqual("Goedemorgen allemaal.", text);
        }

        [TestMethod]
        public void FileInfoClass()
        {
            var fi = new FileInfo("NETFOUNB.Tests.dll");

            Assert.IsTrue(fi.Exists);
            Assert.IsTrue(File.Exists("NETFOUNB.Tests.dll"));
        }

        [TestMethod]
        public void DirectoryInfoClassDemo()
        {
            var di = new DirectoryInfo($@"C:\git");
            di.GetFiles("*.*", SearchOption.AllDirectories);

            di.GetFiles();
        }

        [TestMethod]
        public void PathClassDemo()
        {
            
            Path.Combine(@"C:\git", "output.log");
            var path = Path.ChangeExtension("input.zip", "bak");

            Assert.AreEqual("input.bak", path);
        }

        [TestMethod]
        public void RenameVanFileDemo()
        {
            var fi = new FileInfo("original.txt");
            File.WriteAllText(fi.FullName, "testje");

            Assert.IsTrue(fi.Exists);

            var path = Path.ChangeExtension(fi.FullName, "log");
            File.Delete(path);
            fi.MoveTo(path);

            Assert.IsTrue(fi.Exists);
            Assert.AreEqual("original.log", fi.Name);
        }

        [TestMethod]
        public void StreamsDemo()
        {
            string path = "stream-demo.txt";

            File.Delete(path);

            var stream = File.OpenWrite(path);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("hoi");
                throw new Exception();
            }
        }

        [TestMethod]
        public void MemoryStreamDemo()
        {
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            {
                writer.Write("hallo memory");
            }

            stream.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(stream))
            {
                var text = reader.ReadToEnd();
                Assert.AreEqual("hallo memory", text);
            }
        }
    }
}
