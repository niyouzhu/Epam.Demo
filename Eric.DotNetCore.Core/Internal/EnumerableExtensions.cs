using System;
using System.Collections.Generic;

namespace Eric.DotNetCore.Core
{
    /// <summary>
    ///     Collection specific extension methods are defined in this class.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Performs the specified action on each element of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="collection">The collection on whose elements the action is to be performed.</param>
        /// <param name="action">The <see cref="Action" /> to perform on each element of array.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection" /> or <paramref name="action" /> is null.</exception>
        /// <remarks>
        ///     The <see cref="Action&lt;Task&gt;" /> is a delegate to a method that performs an action on the object passed
        ///     to it. The elements of collection are individually passed to the <see cref="Action&lt;Task&gt;" />.
        /// </remarks>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            Guard.ArgumentNotNull(collection, "collection");
            Guard.ArgumentNotNull(action, "action");
            foreach (var it in collection)
                action(it);
        }
    }
}