using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SortingAlgorithms
{
    public class RandomNumbersGenerator
    {
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly Random _random = new Random();

        public RandomNumbersGenerator(int minValue, int maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public int[] Generate(int n)
        {
            var arr = new int[n];
            for (int i = 0; i < n; i++)
                arr[i] = _random.Next(_minValue, _maxValue);
            return arr;
        }
        
        public int[] GenerateSorted(int n)
        {
            var arr = new SortedSet<int>();
            int current = _minValue;
            for (int i = 0; i < n; i++)
            {
                arr.Add(_random.Next(current, _maxValue));
            }
            return arr.ToArray();
        }
    }
}