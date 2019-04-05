using System.Collections.Generic;

namespace SortingAlgorithms
{
    public interface ISortingAlgorithm
    {
        string Name { get; }
        void Sort(ref int[] numbers);
    }
}