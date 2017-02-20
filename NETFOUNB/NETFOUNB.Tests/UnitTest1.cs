using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private ManualResetEvent _isAangeroepen = new ManualResetEvent(false);

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

        [TestMethod]
        public void UsingStatementDemo1()
        {
            var stream = File.OpenWrite("stream-demo.txt");
            var writer = new StreamWriter(stream);
            try
            {
                writer.Write("hoi");
                throw new Exception();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        [TestMethod]
        public void UsingStatementDemo2()
        {
            var stream = File.OpenWrite("stream-demo.txt");
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("hoi");
                throw new Exception();
            }
        }

        [TestMethod]
        public void StreamsInStreamsDemo()
        {
            var stream = File.OpenWrite("compressed.bin");
            var zip = new GZipStream(stream, CompressionMode.Compress);
            var buffer = new BufferedStream(zip);

            using (var writer = new StreamWriter(buffer))
            {
                for (int i = 0; i < 1000000000; i++)
                {
                    writer.Write("hoi");
                }
            }
        }

        [TestMethod]
        public void FileSystemWatcherDemo()
        {
            using (var watcher = new FileSystemWatcher(Path.GetTempPath()))
            {
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.Filter = "*.txt";
                watcher.Created += CreatedEventHandler;
                watcher.EnableRaisingEvents = true;

                var path = Path.ChangeExtension(Path.GetTempFileName(), "txt");
                File.WriteAllText(path, "watcher demo");
            }

            Assert.IsTrue(_isAangeroepen.WaitOne(TimeSpan.FromMinutes(5)));
        }

        private void CreatedEventHandler(object sender, FileSystemEventArgs e)
        {
            _isAangeroepen.Set();
        }



        [TestMethod]
        public void SerializatieDemo()
        {
            string path = "binary-serializer.bin";
            using (var stream = File.OpenWrite(path))
            {
                Persoon p = CreatePersoon();

                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, p);
            }

            using (var stream = File.OpenRead(path))
            {
                var formatter = new BinaryFormatter();
                var p = (Persoon)formatter.Deserialize(stream);

                Assert.AreEqual("Pietje Puk", p.Naam);
                Assert.IsNotNull(p.Vrienden);
                Assert.AreEqual(p.Vrienden[0].Naam, "Agent Langdraad");
            };
        }

        private static Persoon CreatePersoon()
        {
            return new Persoon
            {
                Naam = "Pietje Puk",
                Vrienden = new[]
                {
                        new Persoon
                        {
                            Naam = "Agent Langdraad"
                        }
                    }
            };
        }

        [TestMethod]
        public void XmlSerializerDemo()
        {
            string path = "binary-serializer.xml";
            using (var stream = File.OpenWrite(path))
            {
                var serializer = new XmlSerializer(typeof(Persoon));
                serializer.Serialize(stream, CreatePersoon());
            }
        }

        [TestMethod]
        public void JsonSerializerDemo()
        {
            string path = "binary-serializer.json";
            var stream = File.OpenWrite(path);

            using (var writer = new StreamWriter(stream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, CreatePersoon());
            }
        }

        [TestMethod]
        public void DataContractSerializer()
        {
            string path = "binary-serializer.wcf";
            using (var stream = File.OpenWrite(path))
            { 
                var serializer = new DataContractSerializer(typeof(Persoon));
                serializer.WriteObject(stream, CreatePersoon());
            }
        }

        [Serializable]
        [XmlRoot("persoon")]
        public class Persoon
        {
            [XmlElement(ElementName = "naam")]
            public string Naam { get; set; }
            [XmlElement("persoon")]
            public Persoon[] Vrienden { get; set; }
        }
    }
}
