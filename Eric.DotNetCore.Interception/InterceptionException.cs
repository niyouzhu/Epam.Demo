using System;

namespace Eric.DotNetCore.Interception
{
    /// <summary>
    /// Exception which is thrown from interceptor.
    /// </summary>
    public class InterceptionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptionException"/> class.
        /// </summary>
        public InterceptionException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InterceptionException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner"> The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InterceptionException(string message, Exception inner) : base(message, inner) { }
    }
}