using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Eric.DotNetCore.Core
{
    /// <summary>
    /// This class is used to perform common argument validation.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Ensure the specified argument is not null.
        /// </summary>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/> is null.</exception>
        public static void ArgumentNotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Ensure the specified argument is not an empty Guid.
        /// </summary>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/> is null.</exception>
        public static void ArgumentNotEmpty(Guid argumentValue, string argumentName)
        {
            if (argumentValue == Guid.Empty)
            {
                throw new ArgumentException("Specified argument value must not be an empty Guid.", argumentName);
            }
        }

        /// <summary>
        /// Ensure the specified argument is not null.
        /// </summary>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/> is null.</exception>
        public static void ArgumentNotNullOrEmpty<T>(IEnumerable<T> argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (!argumentValue.Any())
            {
                throw new ArgumentException("Specified argument value must not be an empty collection.", argumentName);
            }
        }

        /// <summary>
        /// Ensure the specified argument is not null or empty.
        /// </summary>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argumentValue"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/> is white space.</exception>
        public static void ArgumentNotNullOrWhiteSpace(string argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
            if (string.IsNullOrWhiteSpace(argumentValue))
            {
                throw new ArgumentException("Specified argument value must not be a white space string.", argumentName);
            }
        }

        /// <summary>
        /// Ensue argument is derived from the specified base type.
        /// </summary>
        /// <typeparam name="T">The base type from which the specified argument should be derived.</typeparam>
        /// <param name="argumentValue">The argument value.</param>
        /// <param name="argumentName">Name of the argument.</param>
        /// <exception cref="ArgumentException"><paramref name="argumentValue"/> is not derived from the specified base type.</exception>
        public static void ArgumentIsDerievedFrom<T>(Type argumentValue, string argumentName)
        {
            if (!typeof(T).GetTypeInfo().IsAssignableFrom(argumentValue.GetTypeInfo()))
            {
                string template = "The type \"{0}\" cannot be assignable from the specified \"{1}\".";
                throw new ArgumentException(template.Fill(typeof(T).FullName, argumentValue.FullName), argumentName);
            }
        }
    }
}