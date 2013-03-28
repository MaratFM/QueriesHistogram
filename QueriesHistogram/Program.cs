// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Microsoft">
//   2013 Microsoft Corporation
// </copyright>
// <summary>
//   QueriesHistogram.exe console application
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace QueriesHistogram
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Console application launch class
    /// </summary>
    internal class Program
    {
        #region Public Methods and Operators

        /// <summary>
        /// Reads given file line by line
        /// </summary>
        /// <param name="filePath">file path to read</param>
        /// <returns>string lines</returns>
        public static IEnumerable<string> ReadFile(string filePath)
        {
            using (var reader = File.OpenText(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">command line arguments</param>
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: QueriesHistogram.exe path_to_queries_file");
                Environment.Exit(1);
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File does not exist!");
                Environment.Exit(1);
            }

            var groupPercentiles = new[] { 0.3, 0.6 };
            var groupNames = new[] { "head", "body", "tail" };

            var sourceRand = new RandomPicker<string>(100);
            var uniqueRand = new RandomPicker<string>(100);

            var queries = sourceRand.ProxyStream(ReadFile(args[0]));

            Console.WriteLine("\n======================== All queries ========================\n");
            foreach (var item in StatUtils.FreqHistogram(queries, groupPercentiles))
            {
                uniqueRand.Add(item.Item1);
                Console.WriteLine("{0, -5} {1, -5} {2}", groupNames[item.Item3], item.Item2, item.Item1);
            }

            Console.WriteLine("\n======================== Random 100 queries ========================\n");
            foreach (var item in StatUtils.FreqHistogram(sourceRand.GetPickedItems(), groupPercentiles))
            {
                Console.WriteLine("{0, -5} {1, -5} {2}", groupNames[item.Item3], item.Item2, item.Item1);
            }

            Console.WriteLine("\n======================== Random 100 unique queries ========================\n");
            foreach (var item in StatUtils.FreqHistogram(uniqueRand.GetPickedItems(), groupPercentiles))
            {
                Console.WriteLine("{0, -5} {1, -5} {2}", groupNames[item.Item3], item.Item2, item.Item1);
            }

            Console.ReadKey();
        }

        #endregion
    }
}