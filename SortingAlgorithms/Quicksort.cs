using System;
using System.Diagnostics;
using System.Linq;

namespace SortingAlgorithms
{
    public class Quicksort : ISortingAlgorithm
    {
        public void Sort(ref int[] numbers)
        {
            Sort(ref numbers, 0, numbers.Length - 1);
        }

        private static void Sort(ref int[] numbers, int low, int high)
        {
            if (low >= high) return;
            int p = Partition(ref numbers, low, high);
            Sort(ref numbers, low, p);
            Sort(ref numbers, p + 1, high);
        }

        private static int Partition(ref int[] numbers, int low, int high)
        {
            int pivot = numbers[(low + high) / 2];
            int i = low - 1;
            int j = high + 1;
            while (true)
            {
                do
                {
                    ++i;
                } while (numbers[i] < pivot);
                do
                {
                    --j;
                } while (numbers[j] > pivot);

                if (i >= j) return j;
                
                Swap(ref numbers, i, j);
            }

        }

        private static void Swap(ref int[] numbers, int i, int j)
        {
            int tmp = numbers[i];
            numbers[i] = numbers[j];
            numbers[j] = tmp;
        }
    }
}