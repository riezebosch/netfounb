﻿using System;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Threading;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Xunit;

namespace NETFOUNB.Tests
{
    public class UnitTest1 : IDisposable
    {
        private ManualResetEventSlim _isAangeroepen = new ManualResetEventSlim(false);

        [Fact]
        public void Fact1()
        {
            File.WriteAllText("output.txt", "Goedemorgen allemaal.");
            string text = File.ReadAllText("output.txt");

            Assert.Equal("Goedemorgen allemaal.", text);
        }

        [Fact]
        public void FileInfoClass()
        {
            var path = Guid.NewGuid().ToString();
            File.Create(path);

            var fi = new FileInfo(path);

            Assert.True(fi.Exists);
            Assert.True(File.Exists(path));
        }

        [Fact]
        public void DirectoryInfoClassDemo()
        {
            var di = new DirectoryInfo(".");
            di.GetFiles("*.*", SearchOption.AllDirectories);

            di.GetFiles();
        }

        [Fact]
        public void PathClassDemo()
        {
            Path.Combine(Directory.GetDirectoryRoot("."), "output.log");
            var path = Path.ChangeExtension("input.zip", "bak");

            Assert.Equal("input.bak", path);
        }

        [Fact]
        public void RenameVanFileDemo()
        {
            var fi = new FileInfo("original.txt");
            File.WriteAllText(fi.FullName, "testje");

            Assert.True(fi.Exists);

            var path = Path.ChangeExtension(fi.FullName, "log");
            File.Delete(path);
            fi.MoveTo(path);

            Assert.True(fi.Exists);
            Assert.Equal("original.log", fi.Name);
        }

        [Fact]
        public void StreamsDemo()
        {
            string path = "stream-demo.txt";

            File.Delete(path);

            Assert.ThrowsAsync<Exception>(() => 
            {
                var stream = File.OpenWrite(path);
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write("hoi");
                    throw new Exception();
                }
            });
        }

        [Fact]
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
                Assert.Equal("hallo memory", text);
            }
        }

        [Fact]
        public void UsingStatementDemo1()
        {
            var stream = File.OpenWrite("stream-demo.txt");
            var writer = new StreamWriter(stream);
            try
            {
                writer.Write("hoi");
                //throw new Exception();
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        [Fact]
        public void UsingStatementDemo2()
        {
            var stream = File.OpenWrite("stream-demo.txt");
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("hoi");
                //throw new Exception();
            }
        }

        [Fact]
        public void StreamsInStreamsDemo()
        {
            var stream = File.OpenWrite("compressed.bin");
            var zip = new GZipStream(stream, CompressionMode.Compress);
            var buffer = new BufferedStream(zip);

            using (var writer = new StreamWriter(buffer))
            {
                for (int i = 0; i < 10000000; i++)
                {
                    writer.Write("hoi");
                }
            }
        }

        [Fact]
        public void FileSystemWatcherDemo()
        {
            using (var watcher = new FileSystemWatcher(Path.GetTempPath()))
            {
                watcher.NotifyFilter = NotifyFilters.FileName;
                watcher.Filter = "*.txt";
                watcher.Created += CreatedEventHandler;
                watcher.Changed += CreatedEventHandler;
                watcher.Renamed += CreatedEventHandler;

                watcher.EnableRaisingEvents = true;

                var path = Path.Combine(watcher.Path, $"{Guid.NewGuid()}.txt");
                File.WriteAllText(path, "watcher demo");

                Thread.Sleep(1000);
            }

            Assert.True(_isAangeroepen.WaitHandle.WaitOne(TimeSpan.FromSeconds(5)));
        }

        private void CreatedEventHandler(object sender, FileSystemEventArgs e)
        {
            _isAangeroepen.Set();
        }



        // [Fact]
        // public void SerializatieDemo()
        // {
        //     string path = "binary-serializer.bin";
        //     using (var stream = File.OpenWrite(path))
        //     {
        //         Persoon p = CreatePersoon();

        //         var formatter = new BinaryFormatter();
        //         formatter.Serialize(stream, p);
        //     }

        //     using (var stream = File.OpenRead(path))
        //     {
        //         var formatter = new BinaryFormatter();
        //         var p = (Persoon)formatter.Deserialize(stream);

        //         Assert.AreEqual("Pietje Puk", p.Naam);
        //         Assert.IsNotNull(p.Vrienden);
        //         Assert.AreEqual(p.Vrienden[0].Naam, "Agent Langdraad");
        //     };
        // }

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

        [Fact]
        public void XmlSerializerDemo()
        {
            string path = "binary-serializer.xml";
            using (var stream = File.OpenWrite(path))
            {
                var serializer = new XmlSerializer(typeof(Persoon));
                serializer.Serialize(stream, CreatePersoon());
            }
        }

        [Fact]
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

        public void Dispose()
        {
            _isAangeroepen.Dispose();
        }

        // [Fact]
        // public void DataContractSerializer()
        // {
        //     string path = "binary-serializer.wcf";
        //     using (var stream = File.OpenWrite(path))
        //     { 
        //         var serializer = new DataContractSerializer(typeof(Persoon));
        //         serializer.WriteObject(stream, CreatePersoon());
        //     }
        // }

        // [Serializable]
        // [XmlRoot("persoon")]
        public class Persoon
        {
            // [XmlElement(ElementName = "naam")]
            public string Naam { get; set; }
            // [XmlElement("persoon")]
            public Persoon[] Vrienden { get; set; }
        }
    }
}
