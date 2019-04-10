using System.Collections.Generic;

namespace SortingAlgorithms
{
    public class LibrarySort : ISortingAlgorithm
    {
        public string Name => $"Library sort (epsilon: {_epsilon})";
        private readonly float _epsilon;

        public LibrarySort(float epsilon)
        {
            _epsilon = epsilon;
        }

        public void Sort(ref int[] numbers)
        {
            throw new System.NotImplementedException();
        }
    }
}