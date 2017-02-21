﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
