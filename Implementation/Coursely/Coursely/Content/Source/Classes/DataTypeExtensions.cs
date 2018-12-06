using System.Collections.Generic;

namespace Coursely.Content.Classes
{
    /// <summary>
    /// Holds C# extension methods for various data types.
    /// </summary>
    public static class DataTypeExtensions
    {
        /// <summary>
        /// Gets an enumerator as a list.
        /// </summary>
        /// <typeparam name="T">The type of the enumerator.</typeparam>
        /// <param name="enumerator">An enumerator.</param>
        /// <returns>A list containing the elements in the enumerator.</returns>
        public static List<T> AsList<T>(this IEnumerator<T> enumerator)
        {
            List<T> list = new List<T>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }
            return list;
        }

        /// <summary>
        /// Determines whether an array contains an item of the same type.
        /// O(N) in the worst case (item is not in array), 
        /// O(1) in the best case (item is first element in array).
        /// </summary>
        /// <typeparam name="T">The data type of the array.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="item">The item to search for.</param>
        /// <returns>True if the item is contained in the array, false otherwise.</returns>
        public static bool Contains<T>(this T[] array, T item)
        {
            foreach (var type in array)
            {
                if (type.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines if a string is not-a-number.
        /// </summary>
        /// <param name="str">A string to check.</param>
        /// <returns>True if a string is not a number, false otherwise.</returns>
        public static bool IsNaN(this string str) => !int.TryParse(str, out int n);
    }
}