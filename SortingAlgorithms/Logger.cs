namespace SortingAlgorithms
{
    public class Logger
    {
        public delegate void VoidD(string message);
        public event VoidD OnLog;
        public void Log(string message) => OnLog?.Invoke(message);
    }
}