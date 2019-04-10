namespace SortingAlgorithms
{
    public class MergeSort : ISortingAlgorithm
    {
        public string Name => "Merge sort";

        public void Sort(ref int[] numbers)
        {
            if(numbers.Length < 2) return;
            Sort(ref numbers, 0, numbers.Length);
        }

        private void Sort(ref int[] numbers, int low, int high)
        {
            int n = high - low;
            if(n<=1) return;
            int mid = low + n / 2;
            Sort(ref numbers, low, mid);
            Sort(ref numbers, mid, high);
            
            var a = new int[n];
            int i = low, j = mid;
            for (int k = 0; k < n; ++k)
            {
                if (i == mid) a[k] = numbers[j++];
                else if (j == high) a[k] = numbers[i++];
                else if (numbers[j] < numbers[i]) a[k] = numbers[j++];
                else a[k] = numbers[i++];
            }

            for (int k = 0; k < n; k++)
                numbers[low + k] = a[k];
        }

    }
}