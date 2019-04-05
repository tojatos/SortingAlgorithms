using System;
using System.Diagnostics;
using System.IO;
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
        
        public async Task SaveResult(string data, SequenceLength sl, SequenceType st)
        {
            string path = Path.Combine(_savePath, "Results", GetFileName(sl, st)) + ".txt";
            await Save(data, path);
        }

        private async Task Save(string data, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            await File.WriteAllTextAsync(path, data);
            //.ContinueWith(a=>_logger.Log("Written to file " + path), TaskContinuationOptions.OnlyOnRanToCompletion);

        }

        private static string GetDirectoryName(SequenceLength sl, SequenceType st)
            => Path.Combine(sl.Description(), st.Description());

        private static string GetFileName(SequenceLength sl, SequenceType st)
            => sl.Description() + "_" + st.Description();
    }
}