using System;
using System.Linq;

namespace SortingAlgorithms
{
    internal static class Program
    {
        private const int MinGeneratedValue = int.MinValue;
        private const int MaxGeneratedValue = int.MaxValue;
        private static void Main()
        {
            var numbersGenerator = new RandomNumbersGenerator(MinGeneratedValue, MaxGeneratedValue);
            ISortingAlgorithm[] sortingAlgorithms = {
                new Quicksort(), 
            };
            //numbersGenerator.Generate(100000000).ToList().ForEach(Console.WriteLine);
        }
        
        public static void GenerateAll()
        {
            foreach (object sequenceLength in Enum.GetValues(typeof(SequenceLength)))
            {
                foreach (object sequenceType in Enum.GetValues(typeof(SequenceType)))
                {
                    switch (sequenceLength)
                    {
                        case SequenceLength.HundredThousand:
                            break;
                        case SequenceLength.FiveHundredThousand:
                            break;
                        case SequenceLength.Million:
                            break;
                        case SequenceLength.TwoMillion:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                }
            }
        }
    }
}