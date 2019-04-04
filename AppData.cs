using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public class AppData
    {
        private readonly string _savePath;

        public AppData(string savePath)
        {
            _savePath = savePath;
        }

        public void SaveNumbers(string data, int index, SequenceLength sl, SequenceType st)
        {
            string path = Path.Combine(_savePath, GetDirectoryName(sl, st), index.ToString());
            path += ".txt";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            Task t = File.WriteAllTextAsync(path, data);
            t.ContinueWith(a=>Console.WriteLine("Written to file " + path));

        }


        private static string GetDirectoryName(SequenceLength sl, SequenceType st)
            => Path.Combine(sl.Description(), st.Description());
    }
}