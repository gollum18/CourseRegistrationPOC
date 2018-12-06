using System;

namespace Coursely.Content.Cache
{
    /// <summary>
    /// Represents a key-value pair in cache.
    /// </summary>
    /// <typeparam name="K">The type to use for the key.</typeparam>
    /// <typeparam name="V">The type to use for the value.</typeparam>
    public class CacheItem <K, V>
    {
        //
        // FIELDS
        //

        // Stores the key used to lookup the item
        public K Key { get; set; }
        // Stores the value of the cache entry
        public V Value { get; set; }
        /* Used to track the age of the entry, under the aging page scheme
            the entry with the oldest age is paged out first. If multiple pages fit that 
            profile, then one is chosen arbitrarily.
        */
        public int Age { get; set; }
        /* Whether or not this cache entry holds a value
            Technically, cache entries always hold a non-default value I never set them to their default.
            Instead, I use the occupied flag to notify the cache that the entry is available for use,
            which will in turn overwrite the previous value.
        */ 
        public bool Occupied { get; set; }

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Creates a cache entry given a key and value.
        /// </summary>
        /// <param name="key">The key of the cache entry.</param>
        /// <param name="value">The value of the cache entry.</param>
        public CacheItem(K key, V value)
        {
            Key = key;
            Value = value;
            Age = 0;
        }
    }
}