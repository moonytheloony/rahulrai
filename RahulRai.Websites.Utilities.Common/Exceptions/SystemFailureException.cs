namespace RahulRai.Websites.Utilities.Common.Exceptions
{
    #region

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     The system failure exception.
    /// </summary>
    [Serializable]
    public class SystemFailureException : BlogSystemException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemFailureException" /> class.
        /// </summary>
        public SystemFailureException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemFailureException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        public SystemFailureException(string message)
            : base(message)
        {
            //// Implemented through handler
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemFailureException" /> class.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="innerException">
        ///     The inner exception.
        /// </param>
        public SystemFailureException(string message, Exception innerException)
            : base(message, innerException)
        {
            // add implementation
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SystemFailureException" /> class.
        /// </summary>
        /// <param name="serializationInfo">
        ///     The serialization info.
        /// </param>
        /// <param name="streamingContext">
        ///     The streaming context.
        /// </param>
        protected SystemFailureException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            // Add implementation.
        }

        #endregion
    }
}