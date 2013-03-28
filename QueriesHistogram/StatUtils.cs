// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatUtils.cs" company="Microsoft">
//   2013 Microsoft Corporation
// </copyright>
// <summary>
//   Defines the StatUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace QueriesHistogram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Statistical utilities
    /// </summary>
    public class StatUtils
    {
        #region Public Methods and Operators
        /// <summary>
        /// Generates frequency histogram
        /// </summary>
        /// <typeparam name="T">type of items, no requirements</typeparam>
        /// <param name="inputs">any enumerable with items</param>
        /// <param name="percentiles">list of double percentiles (each must be between 0 and 1)</param>
        /// <returns>list of 3 tuples with fields:
        /// * original item
        /// * frequency of item
        /// * index of percentile group
        /// </returns>
        public static IEnumerable<Tuple<T, long, int>> FreqHistogram<T>(IEnumerable<T> inputs, IEnumerable<double> percentiles)
        {
            long total;
            var counts = Frequency(inputs, out total);
            var percEdges = percentiles.Select(p => (long)(p * total)).Concat(new[] { total }).ToArray();
            long accum = 0;
            int percIndex = 0;
            foreach (var pair in counts.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key))
            {
                accum += pair.Value;
                while (accum > percEdges[percIndex])
                {
                    percIndex++;
                }

                yield return new Tuple<T, long, int>(pair.Key, pair.Value, percIndex);
            }
        }

        /// <summary>
        /// Returns unique items with their frequency
        /// </summary>
        /// <typeparam name="T">type of items, no requirements</typeparam>
        /// <param name="inputs">any enumerable with items</param>
        /// <param name="totalCount">output argument for total count of items</param>
        /// <returns>list of pairs item and it's frequency</returns>
        public static IEnumerable<KeyValuePair<T, long>> Frequency<T>(IEnumerable<T> inputs, out long totalCount)
        {
            long total = 0;
            var counts = new Dictionary<T, long>();
            foreach (var item in inputs)
            {
                long value;
                counts.TryGetValue(item, out value);
                counts[item] = value + 1;
                total++;
            }

            totalCount = total;
            return counts;
        }

        #endregion
    }
}