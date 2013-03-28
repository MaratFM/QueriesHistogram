using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QueriesHistogram;

namespace Tests
{
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class TestRandomPicker
    {
        private Random random = new Random();

        private int[] pickFromStream(IEnumerable<int> stream, int count=10)
        {
            var picker = new RandomPicker<int>(count, random);
            var _ = picker.ProxyStream(stream).ToArray();
            return picker.GetPickedItems().ToArray();
        }
        [TestMethod]
        public void TestWorking()
        {
            var stream = Enumerable.Range(0, 100).ToArray();
            var result = pickFromStream(stream, 10);

            Assert.AreEqual(10, result.Count());
            foreach (var i in result)
                Assert.IsTrue(stream.Contains(i));
        }

        private void checkSize(int size, int picked)
        {
            var stream = Enumerable.Range(0, size).ToArray();
            var result = pickFromStream(stream, 10);

            Assert.AreEqual(picked, result.Count());
            foreach (var i in result) 
                Assert.IsTrue(stream.Contains(i));            
        }

        [TestMethod]
        public void TestDifferentSizeCollection()
        {
            checkSize(0, 0);
            checkSize(1, 1);
            checkSize(5, 5);
            checkSize(9, 9);
            checkSize(10, 10);
            checkSize(11, 10);
            checkSize(100, 10);
            checkSize(1000, 10);
        }

        [TestMethod]
        public void TestDistribution()
        {
            var stream = Enumerable.Range(0, 100).ToArray();

            var pickCounts = new Dictionary<int, int>();
            foreach (var item in stream)
                pickCounts[item] = 0;

            for (int i = 0; i < 1000; i++)
                foreach (var item in pickFromStream(stream, 10))
                    pickCounts[item]++;

            var counts = pickCounts.Values.ToArray();
            var avg = counts.Average();
            var std = Math.Sqrt(counts.Select(x => Math.Pow(x - avg, 2)).Sum() / (counts.Count() - 1));
            
            Assert.IsTrue(std<11);
            Assert.IsTrue(std>9);
        }

    }
}
