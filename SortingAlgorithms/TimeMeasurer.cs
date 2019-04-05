using System;
using System.Diagnostics;

namespace SortingAlgorithms
{
    public static class TimeMeasurer
    {
        private static readonly Stopwatch Sw = new Stopwatch();

        public static TimeSpan Measure(Action a)
        {
            Sw.Reset();
            Sw.Start();
            a();
            Sw.Stop();
            
            return Sw.Elapsed;
        }
        
    }
}