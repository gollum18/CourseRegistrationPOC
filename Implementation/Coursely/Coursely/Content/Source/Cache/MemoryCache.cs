using System;
using System.Collections.Generic;

using System.Threading;

namespace Coursely.Content.Cache
{
    /// <summary>
    /// Represents a cache that is stored in a contiguous spot in memory.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="V">The type of the value.</typeparam>
    public class MemoryCache<K, V>
    {
        //
        // CONSTANTS
        //

        /// <summary> 
        /// Represents the default amount of items the cache can hold
        ///  Used when the user does not specify a size, or specifies an invalid size
        /// </summary>
        public static readonly int DEFAULT_SIZE = 32;
        /// <summary>
        /// Returned by the cache when the requested item was not found
        /// </summary>
        public static readonly int NOT_FOUND = -1;

        //
        // ATTRIBUTES
        //

        /// <summary>
        /// Used to control concurrent access to the cache.
        /// </summary>
        private ReaderWriterLockSlim ReadWriteLock = new ReaderWriterLockSlim();
        /// <summary>
        /// Holds the cache entries.
        /// </summary>
        private CacheItem<K, V>[] Cache;
        /// <summary>
        /// The size of the cache.
        /// </summary>
        public int Size { get { return Cache.Length; } }
        /// <summary>
        /// Used as a fallback in case paging doesn't work
        /// </summary>
        private readonly Random PageGenerator = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);

        //
        // CONSTRUCTORS
        //

        /// <summary>
        /// Creates a memory cache of default size (32 entries).
        /// </summary>
        /// <param name="pagePolicy">The page policy associated with the cache.</param>
        public MemoryCache() : this(DEFAULT_SIZE) { }

        /// <summary>
        /// Creates a memory cache with the specified size. An invalid size is any size less than 
        /// 16.
        /// </summary>
        /// <param name="size">The amount of items to store in the cache.</param>
        /// <param name="pagePolicy">The page policy associated with the cache.</param>
        public MemoryCache(int size)
        {
            if (size < DEFAULT_SIZE / 2)
            {
                size = DEFAULT_SIZE;
            }
            Cache = new CacheItem<K, V>[size];
            for (int i = 0; i < size; i++)
            {
                Cache[i] = new CacheItem<K, V>(default(K), default(V));
            }
        }

        //
        // METHODS
        //

        /// <summary>
        /// Adds an item to the cache.
        /// </summary>
        /// <param name="key">The key to index the value.</param>
        /// <param name="value">The value to store in cache.</param>
        public void Add(K key, V value)
        {
            Age();
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                int pos = -1;
                for (int i = 0; i < Size; i++)
                {
                    if (!Cache[i].Occupied)
                    {
                        pos = i;
                        break;
                    }
                }
                if (pos == -1)
                {
                    pos = Page();
                }
                Cache[pos].Key = key;
                Cache[pos].Value = value;
                Cache[pos].Occupied = true;
                Cache[pos].Age = 0;
                ReadWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Ages every entry in the cache by one (1) unit.
        /// </summary>
        private void Age()
        {
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                for (int i = 0; i < Cache.Length; i++)
                {
                    Cache[i].Age++;
                }
                ReadWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Determines whether the cache contains a value with the corresponding key.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>True if there was an entry with the specified key, false otherwise.</returns>
        public bool Contains(K key)
        {
            bool result = false;
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld) {
                for (int i = 0; i < Size; i++)
                {
                    if (Cache[i].Occupied && Cache[i].Key.Equals(key))
                    {
                        result = true;
                    }
                }
                ReadWriteLock.ExitReadLock();
            }
            return result;
        }

        /// <summary>
        /// Gets an item from cache using the specified key.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <returns>A value if it is found.</returns>
        /// <exception cref="Exception">If there is no entry in cache with the specified key.
        /// Signifies a page fault.</exception>
        public V Get(K key)
        {
            Age();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld)
            {
                for (int i = 0; i < Size; i++)
                {
                    if (Cache[i].Occupied && Cache[i].Key.Equals(key))
                    {
                        ReadWriteLock.ExitReadLock();
                        return Cache[i].Value;
                    }
                }
                ReadWriteLock.ExitReadLock();
            }
            throw new Exception($"No item in cache with the key: {key}!");
        }

        /// <summary>
        /// Gets all values currently stored in the cache.
        /// </summary>
        /// <returns>A list of all of the values stored in cache.</returns>
        public List<V> GetValues()
        {
            Age();
            List<V> values = new List<V>();
            ReadWriteLock.EnterReadLock();
            if (ReadWriteLock.IsReadLockHeld) {
                for (int i = 0; i < Size; i++)
                {
                    if (Cache[i].Occupied)
                    {
                        Cache[i].Age = 0;
                        values.Add(Cache[i].Value);
                    }
                }
                ReadWriteLock.ExitReadLock();
            }
            return values;
        }

        /// <summary>
        /// Gets a position in cache to page from. Implements an aging mechanism for paging.
        /// </summary>
        /// <returns>An position to page from.</returns>
        private int Page()
        {
            int pos = 0;
            int age = Cache[pos].Age;
            for (int i = 1; i < Size; i++)
            {
                if (Cache[i].Age > age)
                {
                    pos = i;
                    age = Cache[i].Age;
                }
            }
            return pos;
        }

        /// <summary>
        /// Removes the first entry in cache with the specified key.
        /// </summary>
        /// <param name="key">A key to find.</param>
        public void Remove(K key)
        {
            Age();
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                for (int i = 0; i < Size; i++)
                {
                    if (Cache[i].Occupied && Cache[i].Key.Equals(key))
                    {
                        Cache[i].Occupied = false;
                        Cache[i].Age = 0;
                    }
                }
                ReadWriteLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Resets the cache to an empty state.
        /// </summary>
        public void Reset()
        {
            foreach (var item in Cache)
            {
                item.Age = 0;
                item.Occupied = false;
            }
        }

        /// <summary>
        /// Updates a value in cache with the specified value using the specified key.
        /// </summary>
        /// <param name="key">The key to find.</param>
        /// <param name="value">The value to update the entry to.</param>
        public void Set(K key, V value)
        {
            Age();
            ReadWriteLock.EnterWriteLock();
            if (ReadWriteLock.IsWriteLockHeld) {
                for (int i = 0; i < Size; i++)
                {
                    if (Cache[i].Occupied && Cache[i].Key.Equals(key))
                    {
                        Cache[i].Value = value;
                        Cache[i].Age = 0;
                        break;
                    }
                }
                ReadWriteLock.ExitWriteLock();
            }
        }
    }
}