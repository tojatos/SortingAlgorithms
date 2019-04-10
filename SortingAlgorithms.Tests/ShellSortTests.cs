using NUnit.Framework;

namespace SortingAlgorithms.Tests
{
    [TestFixture]
    public class ShellSortTests
    {
        [Test]
        [TestCase(new int[]{})]
        [TestCase(new[]{0})]
        [TestCase(new[]{1, 2, 3})]
        [TestCase(new[]{5, 2, 1})]
        [TestCase(new[]{-234, 234, 23, 234234, -234, 0, 0, 0, 15, -13})]
        [TestCase(new[]{15, 3242, 234, 34, 68, 34, 35})]
        public void CollectionSortedShell(int[] numbers)
        {
            new ShellSort(ShellSortSeries.Shell).Sort(ref numbers);
            CollectionAssert.IsOrdered(numbers);
        }
        
        [Test]
        [TestCase(new int[]{})]
        [TestCase(new[]{0})]
        [TestCase(new[]{1, 2, 3})]
        [TestCase(new[]{5, 2, 1})]
        [TestCase(new[]{-234, 234, 23, 234234, -234, 0, 0, 0, 15, -13})]
        [TestCase(new[]{15, 3242, 234, 34, 68, 34, 35})]
        public void CollectionSortedFrankLazarus(int[] numbers)
        {
            new ShellSort(ShellSortSeries.FrankLazarus).Sort(ref numbers);
            CollectionAssert.IsOrdered(numbers);
        }
    }
}