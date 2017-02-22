using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceCountersDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreatePerformanceCounters();
            var counters = RetrieveCounters();

            ExecuteUntilQuit(counters);
        }

        private static void ExecuteUntilQuit(Dictionary<string, PerformanceCounter> counters)
        {
            var key = Console.ReadKey();
            while (key.Key != ConsoleKey.Q)
            {
                PerformanceCounter counter;
                if (!counters.TryGetValue(key.Key.ToString(), out counter))
                {
                    counter = counters["#"];
                }

                counter.IncrementBy(10);
                key = Console.ReadKey();
            }
        }

        private static Dictionary<string, PerformanceCounter> RetrieveCounters()
        {
            var category = PerformanceCounterCategory
                            .GetCategories()
                            .First(pc => pc.CategoryName == "demo");

            var counters = category
                .GetCounters()
                .ToDictionary(c => c.CounterName.ToUpper());

            counters
                .Values
                .ToList()
                .ForEach(c => c.ReadOnly = false);

            return counters;
        }

        private static void CreatePerformanceCounters()
        {
            if (!PerformanceCounterCategory.Exists("demo"))
            {
                var collection = new CounterCreationDataCollection();
                collection.Add(new CounterCreationData("A", "Key A was pressed", PerformanceCounterType.SampleCounter));
                collection.Add(new CounterCreationData("S", "Key S was pressed", PerformanceCounterType.SampleCounter));
                collection.Add(new CounterCreationData("D", "Key D was pressed", PerformanceCounterType.SampleCounter));
                collection.Add(new CounterCreationData("F", "Key F was pressed", PerformanceCounterType.SampleCounter));
                collection.Add(new CounterCreationData("#", "Another key was pressed", PerformanceCounterType.SampleCounter));

                PerformanceCounterCategory.Create("demo", "demo for performance counters", PerformanceCounterCategoryType.SingleInstance, collection);
            }
        }
    }
}
