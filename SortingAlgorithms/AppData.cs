using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public class AppData
    {
        private readonly string _savePath;
        private readonly Logger _logger;

        public AppData(string savePath, Logger logger)
        {
            _savePath = savePath;
            _logger = logger;
        }

        public bool AreNumbersAlreadySaved(SequenceLength sl, SequenceType st)
        {
            string path = Path.Combine(_savePath, GetDirectoryName(sl, st));
            if (!Directory.Exists(path)) return false;
            return new DirectoryInfo(path).GetFiles().Length == 100;
        }
        public async Task SaveNumbers(string data, int index, SequenceLength sl, SequenceType st)
        {
            string path = Path.Combine(_savePath, GetDirectoryName(sl, st), index.ToString()) + ".txt";
            await Save(data, path);
        }

        public int[] GetNumbers(int index, SequenceLength sl, SequenceType st)
        {
            string path = Path.Combine(_savePath, GetDirectoryName(sl, st), index.ToString()) + ".txt";
            return File.ReadAllText(path).Split(' ').Select(int.Parse).ToArray();
        }
        public async Task SaveResult(string data, ISortingAlgorithm algorithm,  SequenceLength sl, SequenceType st)
        {
            string path = Path.Combine(_savePath, "Results", GetFileName(algorithm, sl, st)) + ".txt";
            await Save(data, path);
        }

        private static async Task Save(string data, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            await File.WriteAllTextAsync(path, data);
        }

        private static string GetDirectoryName(SequenceLength sl, SequenceType st)
            => Path.Combine(sl.Description(), st.Description());

        private static string GetFileName(SequenceLength sl, SequenceType st)
            => sl.Description() + "_" + st.Description();
        
        private static string GetFileName(ISortingAlgorithm algorithm, SequenceLength sl, SequenceType st)
            => algorithm.Name + "_" + GetFileName(sl, st);
    }
}