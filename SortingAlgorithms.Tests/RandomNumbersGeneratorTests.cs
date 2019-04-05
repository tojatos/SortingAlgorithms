using NUnit.Framework;

namespace SortingAlgorithms.Tests
{
    [TestFixture]
    public class RandomNumbersGeneratorTests
    {
        private readonly RandomNumbersGenerator _rng;
        private const int Min = -5;
        private const int Max = 5;
        
        public RandomNumbersGeneratorTests()
        {
            _rng = new RandomNumbersGenerator(Min, Max);
        }

        [Test]
        public void GenerationInRange()
        {
            int[] numbers = _rng.Generate(5000);
            Assert.That(numbers, Has.All.GreaterThanOrEqualTo(Min));
            Assert.That(numbers, Has.All.LessThan(Max));
        }
        
        [Test]
        public void GenerationCount()
        {
            int[] numbers = _rng.Generate(5000);
            Assert.That(numbers.Length, Is.EqualTo(5000));
        }
    }
}