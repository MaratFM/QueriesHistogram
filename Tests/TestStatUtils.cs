using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    using System.Collections.Generic;

    using QueriesHistogram;

    [TestClass]
    public class TestStatUtils
    {
        [TestMethod]
        public void TestFrequency()
        {
            long total;
            var freq = StatUtils.Frequency<int>(new[] { 1, 1, 2, 1, 3, 2, 3, 4, 5, 1 }, out total);
            var valid = new [] {
                (new KeyValuePair<int, long>(1, 4)),
                (new KeyValuePair<int, long>(2, 2)),
                (new KeyValuePair<int, long>(3, 2)),
                (new KeyValuePair<int, long>(4, 1)),
                (new KeyValuePair<int, long>(5, 1)),
            };
            int i = 0;
            foreach(var pair in freq)
            {
                Assert.AreEqual(valid[i], pair);
                i++;
            }
        }

        [TestMethod]
        public void TestFreqHistogram()
        {
            var input = new[] { 1, 1, 2, 1, 3, 2, 3, 4, 5, 1 };
            var freq = StatUtils.FreqHistogram<int>(input, new[]{0.1, 0.5});
            var valid = new[] {
                (new Tuple<int, long, int>(1, 4, 1)),
                (new Tuple<int, long, int>(2, 2, 2)),
                (new Tuple<int, long, int>(3, 2, 2)),
                (new Tuple<int, long, int>(4, 1, 2)),
                (new Tuple<int, long, int>(5, 1, 2)),
            };
            int i = 0;
            foreach (var pair in freq)
            {
                Assert.AreEqual(valid[i], pair);
                i++;
            }
        }

    }
}
