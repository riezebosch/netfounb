using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace NETFOUNB.Tests
{
    [TestClass]
    public class ReflectionAndAttributesDemo
    {
        [TestMethod]
        public void TestMethod1()
        {
            bool heeftId = HeeftDezeClassEenIdAttribute(typeof(Person));
            
            Assert.IsTrue(heeftId);
        }

        [TestMethod]
        public void MetReflectionTypesInstantieren()
        {
            var p = Create<Person>();
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void MetReflectionTypesInstantierenZonderGenerics()
        {
            var type = typeof(Person);
            var p = Create(type);
            Assert.IsNotNull(p);

            type.GetMethod("ToString").Invoke(p, null);
        }

        private object Create(Type type)
        {
            return Activator.CreateInstance(type);
        }

        private T Create<T>()
        {
            return Activator.CreateInstance<T>();
        }

        private static bool HeeftDezeClassEenIdAttribute(Type t)
        {
            //return t.GetProperties()
            //    .Any(p => p.GetCustomAttributes(typeof(IdAttribute), false).Any());

            var properties = t.GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(IdAttribute), false);
                if (attributes.Length > 0)
                {
                    return true;
                }
            }

            return false; 
        }

        private class IdAttribute : Attribute
        {
        }

        private class Person
        {
            private int _leeftijd = 31;

            [Id]
            public string BSN { get; set; }

            public bool ControleerLeeftijd(int verwacht)
            {
                return _leeftijd == verwacht;
            }
        }

        [TestMethod]
        public void ReflectionOpPrivateMembers()
        {
            var p = new Person();
            var field = typeof(Person)
                .GetField("_leeftijd",
                BindingFlags.NonPublic | BindingFlags.Instance);

            int leeftijd = (int)field.GetValue(p);
            Assert.AreEqual(31, leeftijd);

            field.SetValue(p, 32);
            Assert.IsTrue(p.ControleerLeeftijd(32));
        }
    }
}
