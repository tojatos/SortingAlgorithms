using System;
using System.Collections.Generic;
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
        
        private static void Main()
        {
            var logger = new Logger();
            logger.OnLog += Console.WriteLine;
            var appData = new AppData(SavePath, logger);
            var rng = new RandomNumbersGenerator(MinGeneratedValue, MaxGeneratedValue);
            ISortingAlgorithm[] sortingAlgorithms = {
                new Quicksort(),
                new ShellSort(),
                new LibrarySort(),
            };
            while(true) {
                Console.WriteLine("Welcome to the sorting algorithm simulation engine!");
                Console.WriteLine($"Data path: {SavePath}");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("(1) - Start full simulation");
                Console.WriteLine("(2) - Simulate one case");
                Console.WriteLine("(3) - Exit");
                string k = Console.ReadLine();
                Console.WriteLine(k);

                switch (k)
                {
                    case "1":
                        StartFullSimulation(appData, logger);
                        break;
                    case "2":
                        SimulateOneCase(appData, logger);
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

        private static void SimulateOneCase(AppData appData, Logger logger)
        {
            Console.Clear();
            Console.WriteLine("Not avaiable yet!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void StartFullSimulation(AppData appData, Logger logger)
        {
            Console.Clear();
            TimeSpan generationAndWriteTime = TimeMeasurer.Measure(() => GenerateAndSaveAll(appData, logger));
            Console.WriteLine($"Data generated and saved in {generationAndWriteTime.TotalSeconds} seconds.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void GenerateAndSaveAll(AppData appData, Logger logger)
        {
            var qs = new Quicksort();
            var writeTasks = new List<Task>();
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
                            var rng = new RandomNumbersGenerator(MinGeneratedValue, MaxGeneratedValue);
                            int[] arr = rng.Generate((int) sequenceLength);
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
                            await appData.SaveNumbers(string.Join(' ', arr), i, sequenceLength, sequenceType);
                        }
                    );
                    Task.WaitAll(writeTasks.ToArray());
                    logger.Log($"{sequenceLength.Description()} {sequenceType.Description()} - writing finished.");
                }
            }
        }
    }
}