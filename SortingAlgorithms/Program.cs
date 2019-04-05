using System;
using System.IO;
using System.Linq;

namespace SortingAlgorithms
{
    internal static class Program
    {
        private const int MinGeneratedValue = int.MinValue;
        private const int MaxGeneratedValue = int.MaxValue;
        private static readonly string SavePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SortingAlgorithms");
        
        private static void Main()
        {
            var appData = new AppData(SavePath);
            var numbersGenerator = new RandomNumbersGenerator(MinGeneratedValue, MaxGeneratedValue);
            ISortingAlgorithm[] sortingAlgorithms = {
                new Quicksort(), 
            };
            GenerateAndSaveAll(numbersGenerator, appData);
        }

        private static void GenerateAndSaveAll(RandomNumbersGenerator gen, AppData appData)
        {
            var qs = new Quicksort();
            foreach (SequenceLength sequenceLength in Enum.GetValues(typeof(SequenceLength)))
            {
                foreach (SequenceType sequenceType in Enum.GetValues(typeof(SequenceType)))
                {
                    int[] arr = gen.Generate((int)sequenceLength);
                    switch (sequenceType)
                    {
                        case SequenceType.HalfSorted:
                            int[] firstHalf = arr.Take(arr.Length / 2).ToArray();
                            int[] secondHalf = arr.Skip(arr.Length / 2).ToArray();
                            qs.Sort(ref firstHalf);
                            arr = firstHalf.Concat(secondHalf).ToArray();
                            break;
                        case SequenceType.Sorted:
                        case SequenceType.ReverseSorted:
                            qs.Sort(ref arr);
                            break;
                    }

                    if (sequenceType == SequenceType.ReverseSorted) arr = arr.Reverse().ToArray();
                    
                    for (int i = 0; i < 100; ++i)
                    {
                        appData.SaveNumbers(string.Join(' ', arr), i, sequenceLength, sequenceType);
                    }
                    
                }
            }
        }
    }
}