using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class LijstjesDemo
    {
        [TestMethod]
        public void WerkenMetEenArray()
        {
            // Dit is een 1 dimensionaal lijstje
            int[] items = new int[6];
            items[3] = 15;

            // Dit is een 2 dimensionaal lijstje
            int[,] multi = new int[6, 2];
            multi[2, 1] = 1234;

            // Dit is een lijstje van lijstjes
            int[][] jagged = new int[6][];
            jagged[2] = new int[14];
            jagged[1] = new int[2];

            int[] korter = { 1, 2, 3, 2, 4, 3423 };
            int[] hetzelfde = new int[6];
            hetzelfde[0] = 1;
            hetzelfde[1] = 2;
            hetzelfde[2] = 3;
            hetzelfde[3] = 2;
            hetzelfde[4] = 4;
            hetzelfde[5] = 3423;

            // Nadeel van arrays: handmatig groter maken
            int[] groter = new int[7];
            Array.Copy(hetzelfde, groter, hetzelfde.Length);
            groter[6] = 2;

            object[] array = new object[2];
            array[0] = "één";
            array[1] = 1;

            int item = (int)array[1];

            MethodeMetEenParamsArray(new[] { 1, 2, 3, 4, 5, 6 });
            MethodeMetEenParamsArray(1, 2, 3, 4, 5, 6);

            Console.WriteLine("Format: {0}, argumenten {1}", 123123, "twee");
        }

        private void MethodeMetEenParamsArray(params int[] args)
        {
        }

        [TestMethod]
        public void DemoVanEenGenericList()
        {
            var items = new List<int>();
            items.Add(3);
            items.Add(1);
            items.Add(20);

            var item = items[2];

            var korter = new List<int> { 1, 2, 34, 4, 54, 34, 34 };
            var hetzelfde = new List<int>();
            hetzelfde.Add(1);
            hetzelfde.Add(2);
            hetzelfde.Add(34);
            hetzelfde.Add(4);
            hetzelfde.Add(54);
            hetzelfde.Add(34);
            hetzelfde.Add(34);

            // Een array list voegt niets toe boven een List
            var ongetypeerd = new ArrayList();
            ongetypeerd.Add(5);
            int o = (int)ongetypeerd[0];
        }

        [TestMethod]
        public void WatIsDanEenDictionary()
        {
            var dict = new Dictionary<string, int>();
            dict["asdf"] = 5;
            dict["qwer"] = 2134234;

            var item = dict["asdf"];
            Assert.AreEqual(5, item);

            var korter = new Dictionary<string, int>
            {
                { "asdf", 5 },
                {"qewr", 6 }
            };

            var csharp6 = new Dictionary<string, int>
            {
                ["asdf"] = 5,
                ["qwer"] = 6
            };

            // Dit geeft een exception
            //var bestaatniet = dict["13245"];
            int bestaatniet;

            if (dict.ContainsKey("12345"))
            {
                bestaatniet = dict["12345"];
            }
            else
            {
                bestaatniet = -1;
            }

            int bestaatniet2 = -1;
            if (!dict.TryGetValue("12345", out bestaatniet2))
            {

            }
        }

        [TestMethod]
        public void DictionaryMetEigenKeys()
        {
            var eigenkey = new Dictionary<MyKey, int>();
            eigenkey[new MyKey { Property1 = "Pietje", Property2 = 3 }] = 5;

            var eigen = eigenkey[new MyKey { Property1 = "Pietje", Property2 = 3 }];
            Assert.AreEqual(5, eigen);

            var metcomparer = new Dictionary<MyKey2, int>(new MyComparer());
            metcomparer[new MyKey2 { Property1 = "Pietje" }] = 23;

            Assert.AreEqual(23, metcomparer[new MyKey2 { Property1 = "Pietje" }]);
        }

        public void DezeVoldoetAanDeSignatuurVanDeForeachMethodeOpMijnList(int i)
        {

        }

        [TestMethod]
        public void HandigeDelegateMethodeOpList()
        {
            var items = new List<int> { 123, 23, 234, 234, 234 };

            // Niet zo
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }

            // Maar zo
            items.ForEach(i => Console.WriteLine(i));

            items.ForEach(new Action<int>(DezeVoldoetAanDeSignatuurVanDeForeachMethodeOpMijnList));
            items.ForEach(DezeVoldoetAanDeSignatuurVanDeForeachMethodeOpMijnList);
            items.ForEach(delegate (int i) { Console.WriteLine(i); });
            items.ForEach((int i) => { Console.WriteLine(i); });
            items.ForEach(i => { Console.WriteLine(i); });
            items.ForEach(i => Console.WriteLine(i));
            items.ForEach(Console.WriteLine);

            bool iserookeenkleinerdan0 = items.Any(p => p < 0);
            Assert.IsFalse(iserookeenkleinerdan0);

            bool allesgroterdan0 = items.All(p => p >= 0);
            Assert.IsTrue(allesgroterdan0);

            // Misschien ten overvloede
            var ditisnietwatjeverwacht = items.ToString();
            Assert.AreNotEqual("123, 23, 234, 234, 234", ditisnietwatjeverwacht);

            var ditiskorterdanforeachen = string.Join(", ", items);
            Assert.AreEqual("123, 23, 234, 234, 234", ditiskorterdanforeachen);
        }

        [TestMethod]
        public void HoeKijkJeOfEenDoubleDecimalenBevat()
        {
            decimal a = 3.0m;
            Assert.IsFalse(HeeftDecimalen(a));

            decimal b = 3.1m;
            Assert.IsTrue(HeeftDecimalen(b));
        }

        [TestMethod]
        public void WatIsZoUniekAanEenDecimal()
        {
            decimal a = 100002m / 3;
            Assert.AreEqual(100002m, a * 3);
        }

        [TestMethod]
        public void DoubleVoorbeeld()
        {
            double a = 0.1,
                b = 0.2,
                c = a + b;
            Assert.AreNotEqual(0.3, c);

            decimal d = 0.1m,
                e = 0.2m,
                f = d + e;
            Assert.AreEqual(0.3m, f);
        }

        private bool HeeftDecimalen(decimal a)
        {
            return (a % 1) != 0;
        }

        private class MyKey
        {
            public int Property2 { get; internal set; }
            public string Property1 { get; internal set; }

            public override int GetHashCode()
            {
                return Property1.GetHashCode() ^ Property2.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var rhs = obj as MyKey;
                if (rhs == null)
                {
                    return false;
                }

                return Property1.Equals(rhs.Property1) &&
                    Property2.Equals(rhs.Property2);
            }
        }

        private class MyKey2
        {
            public string Property1 { get; internal set; }
        }

        private class MyComparer : IEqualityComparer<MyKey2>
        {
            public bool Equals(MyKey2 x, MyKey2 y)
            {
                return x.Property1.Equals(y.Property1);
            }

            public int GetHashCode(MyKey2 obj)
            {
                return obj.Property1.GetHashCode();
            }
        }

        [TestMethod]
        public void VoorbeeldjeVanEenOneindigeLijst()
        {
            foreach (var item in Fib())
            {
                if (item > 1000)
                {
                    break;
                }
                Console.WriteLine(item);
            }
        }

        IEnumerable<int> Fib()
        {
            int a = 0;
            yield return a;

            int b = 1;
            yield return b;

            while (true)
            {
                var t = a + b;
                yield return t;

                a = b;
                b = t;
            }
        }

        [TestMethod]
        public void PLinqDemo()
        {
            int[] items = { 1, 4, 2, 5, 3, 4, 6, 3, 76, 45, 23, 3, 23, 65, 34, 34, 67, 345, 235, 74356, 345, 345, 2, 1234 };

            Console.WriteLine("sequential:");
            Console.WriteLine(string.Join(", ", items
                .Where(i => i % 2 == 0)));

            Console.WriteLine("parallel:");
            Console.WriteLine(string.Join(", ", items
                .AsParallel()
                .AsOrdered()
                .Where(i => i % 2 == 0)));
        }

        [TestMethod]
        public void PLinqDemoPerf()
        {
            var r = new Random();
            var items = Enumerable.Range(0, 100).Select(i => r.Next());

            var sw = Stopwatch.StartNew();
            Console.WriteLine("sequential:");
            Func<int, bool> pred = i => { /*Thread.Sleep(1);*/ return i % 2 == 0; };
            items.Where(pred).Last();
            Console.WriteLine(sw.Elapsed);

            sw.Restart();
            Console.WriteLine("parallel:");
            items.AsParallel().Where(pred).Last();
            Console.WriteLine(sw.Elapsed);
        }

        private Lazy<IList<int>> _items = new Lazy<IList<int>>(() => new List<int>(), true);

        public IList<int> Items
        {
            get
            {
                return _items.Value;
            }
        }


        [TestMethod]
        public void LazyInitializationDemo()
        {
            Items.Add(4);

            Parallel.Invoke(
                () => Items.Add(4), 
                () => Items.Add(2));
        }
    }
}
