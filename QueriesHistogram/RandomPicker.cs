// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomPicker.cs" company="Microsoft">
//   2013 Microsoft Corporation
// </copyright>
// <summary>
//   Defines the RandomPicker type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace QueriesHistogram
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// RandomPicker - picks random K items from collection. Might be used with streams.
    /// </summary>
    /// <typeparam name="T">type of the collection items</typeparam>
    public class RandomPicker<T>
    {
        #region Fields

        /// <summary>
        /// maximum number of picked items
        /// </summary>
        private readonly int maxCount;

        /// <summary>
        /// list of picked items, used as heap
        /// </summary>
        private readonly SortedList<int, T> pickedItems;

        /// <summary>
        /// random generator
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// current minimal weight stored in pickedItems 
        /// </summary>
        private int minimal;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initialize a new instance of the <see cref="RandomPicker{T}"/> class.
        /// </summary>
        /// <param name="count">number of random items to pick</param>
        public RandomPicker(int count)
            : this(count, new Random())
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RandomPicker{T}"/> class.
        /// </summary>
        /// <param name="count">number of random items to pick</param>
        /// <param name="random">random generator</param>
        public RandomPicker(int count, Random random)
        {
            this.maxCount = count;
            this.pickedItems = new SortedList<int, T>();
            this.random = random;
            this.minimal = int.MinValue;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Add item to picker
        /// </summary>
        /// <param name="item">some item</param>
        public void Add(T item)
        {
            var weight = this.random.Next();
            if (weight > this.minimal)
            {
                this.pickedItems.Add(weight, item);
                if (this.pickedItems.Count > this.maxCount)
                {
                    this.pickedItems.RemoveAt(0);
                    this.minimal = this.pickedItems.First().Key;
                }
            }
        }

        /// <summary>
        /// Returns already picked items
        /// </summary>
        /// <returns>picked items</returns>
        public IEnumerable<T> GetPickedItems()
        {
            return this.pickedItems.Select(pair => pair.Value);
        }

        /// <summary>
        /// Connects to items stream pick random from it and yields stream forward
        /// </summary>
        /// <param name="input">input stream</param>
        /// <returns>same stream as input</returns>
        public IEnumerable<T> ProxyStream(IEnumerable<T> input)
        {
            foreach (var item in input)
            {
                this.Add(item);
                yield return item;
            }
        }

        #endregion
    }
}