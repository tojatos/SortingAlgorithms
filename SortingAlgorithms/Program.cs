using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    internal static class Program
    {
        private const int MinGeneratedValue = int.MinValue;
        private const int MaxGeneratedValue = int.MaxValue;
        private static readonly string SavePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SortingAlgorithms");
        
        private static readonly List<ISortingAlgorithm> SortingAlgorithms = new List<ISortingAlgorithm> {
            new Quicksort(),
            new ShellSort(ShellSortSeries.Shell),
            new ShellSort(ShellSortSeries.FrankLazarus),
            new MergeSort(),
        };
        
        private static void Main()
        {
            var logger = new Logger();
            logger.OnLog += Console.WriteLine;
            var appData = new AppData(SavePath, logger);
            while(true) {
                Console.WriteLine("Welcome to the sorting algorithm simulation engine!");
                Console.WriteLine($"Data path: {SavePath}");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1 - Start full simulation");
                Console.WriteLine("2 - Simulate one case");
                Console.WriteLine("3 - Exit");
                string k = Console.ReadLine();

                switch (k)
                {
                    case "1":
                        StartFullSimulation(appData, logger);
                        break;
                    case "2":
                        SimulateOneCase();
                        break;
                    case "3": 
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input!");
                        continue;
                }
                Console.Clear();
            }
        }

        private static void SimulateOneCase()
        {
            ISortingAlgorithm selectedAlgorithm = SelectAlgorithm();
            SequenceLength selectedLength = SelectLength();
            SequenceType selectedType = SelectType();
            Console.WriteLine($"Selected {selectedAlgorithm.Name}, {selectedLength.Description()}, {selectedType.Description()}");
            Console.WriteLine("Sorting...");
            int[] arr = GenerateNumbers(selectedLength, selectedType);
            TimeSpan x = TimeMeasurer.Measure(() => selectedAlgorithm.Sort(ref arr));
            Console.WriteLine($"Array sorted in {x.TotalSeconds} seconds.");
            
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static SequenceType SelectType()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please select sequence type:");
                Console.WriteLine("1 - Random");
                Console.WriteLine("2 - Half Sorted");
                Console.WriteLine("3 - Sorted");
                Console.WriteLine("4 - Reverse Sorted");
                string k = Console.ReadLine();

                switch (k)
                {
                    case "1": return SequenceType.Random;
                    case "2": return SequenceType.HalfSorted;
                    case "3": return SequenceType.Sorted;
                    case "4": return SequenceType.ReverseSorted;
                    default:
                        continue;
                }
            }
        }

        private static SequenceLength SelectLength()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please select sequence length:");
                Console.WriteLine("1 - Hundred Thousand");
                Console.WriteLine("2 - Five Hundred Thousand");
                Console.WriteLine("3 - Million");
                Console.WriteLine("4 - Two Million");
                string k = Console.ReadLine();

                switch (k)
                {
                    case "1": return SequenceLength.HundredThousand;
                    case "2": return SequenceLength.FiveHundredThousand;
                    case "3": return SequenceLength.Million;
                    case "4": return SequenceLength.TwoMillion;
                    default:
                        continue;
                }
            }
        }

        private static ISortingAlgorithm SelectAlgorithm()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please select sorting algorithm:");
                for (int i = 0; i < SortingAlgorithms.Count; i++)
                    Console.WriteLine($"{i + 1} - {SortingAlgorithms[i].Name}");
                bool parsed = int.TryParse(Console.ReadLine(), out int k);
                --k;
                if (!parsed || SortingAlgorithms.ElementAtOrDefault(k) == null) continue;
                return SortingAlgorithms[k];
            }
        }

        private static void StartFullSimulation(AppData appData, Logger logger)
        {
            Console.Clear();
            TimeSpan generationAndWriteTime = TimeMeasurer.Measure(() => GenerateAndSaveAll(appData, logger));
            Console.WriteLine($"Data generated and saved in {generationAndWriteTime.TotalSeconds} seconds.");


            foreach (ISortingAlgorithm algorithm in SortingAlgorithms)
            {
                foreach (SequenceLength sequenceLength in Enum.GetValues(typeof(SequenceLength)))
                {
                    foreach (SequenceType sequenceType in Enum.GetValues(typeof(SequenceType)))
                    {
                        Console.WriteLine($"Simulating {algorithm.Name} - {sequenceLength} - {sequenceType}");
                        var times = new double[100];
                        for (int i = 0; i < 100; ++i)
                        {
                            int[] numbers = appData.GetNumbers(i, sequenceLength, sequenceType);
                            double timeInSeconds = TimeMeasurer.Measure(() => algorithm.Sort(ref numbers)).TotalSeconds;
                            times[i] = timeInSeconds;
                        }

                        appData.SaveResult(string.Join(' ', times), algorithm, sequenceLength, sequenceType).Wait();
                    }
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GenerateAndSaveAll(AppData appData, Logger logger)
        {
            foreach (SequenceLength sequenceLength in Enum.GetValues(typeof(SequenceLength)))
            {
                foreach (SequenceType sequenceType in Enum.GetValues(typeof(SequenceType)))
                {
                    if (appData.AreNumbersAlreadySaved(sequenceLength, sequenceType))
                    {
                        logger.Log($"{sequenceLength.Description()} {sequenceType.Description()} - already written - skipping.");
                        continue;
                    }
                    
                    //Partition generation and saving into all cores
                    //MaxDegreeOfParallelism is needed, because we perform many IO operations
                    Parallel.For(0, 100, new ParallelOptions {MaxDegreeOfParallelism = Environment.ProcessorCount}, async i =>
                        {
                            int[] arr = GenerateNumbers(sequenceLength, sequenceType);
                            await appData.SaveNumbers(string.Join(' ', arr), i, sequenceLength, sequenceType);
                        }
                    );
                    logger.Log($"{sequenceLength.Description()} {sequenceType.Description()} - writing finished.");
                }
            }
        }

        private static int[] GenerateNumbers(SequenceLength sequenceLength, SequenceType sequenceType)
        {
            var rng = new RandomNumbersGenerator(MinGeneratedValue, MaxGeneratedValue);
            int[] arr = rng.Generate((int) sequenceLength);
            switch (sequenceType)
            {
                case SequenceType.HalfSorted:
                    int[] firstHalf = arr.Take(arr.Length / 2).ToArray();
                    int[] secondHalf = arr.Skip(arr.Length / 2).ToArray();
                    SortingAlgorithms[0].Sort(ref firstHalf);
                    arr = firstHalf.Concat(secondHalf).ToArray();
                    break;
                case SequenceType.Sorted:
                case SequenceType.ReverseSorted:
                    SortingAlgorithms[0].Sort(ref arr);
                    break;
            }

            if (sequenceType == SequenceType.ReverseSorted) arr = arr.Reverse().ToArray();
            return arr;
        }
    }
}
