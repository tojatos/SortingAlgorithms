using NUnit.Framework;

namespace SortingAlgorithms.Tests
{
    [TestFixture]
    public class MergeSortTests
    {
        [Test]
        [TestCase(new int[]{})]
        [TestCase(new[]{0})]
        [TestCase(new[]{1, 2, 3})]
        [TestCase(new[]{5, 2, 1})]
        [TestCase(new[]{-234, 234, 23, 234234, -234, 0, 0, 0, 15, -13})]
        [TestCase(new[]{15, 3242, 234, 34, 68, 34, 35})]
        public void CollectionSorted(int[] numbers)
        {
            new MergeSort().Sort(ref numbers);
            CollectionAssert.IsOrdered(numbers);
        }
    }
}