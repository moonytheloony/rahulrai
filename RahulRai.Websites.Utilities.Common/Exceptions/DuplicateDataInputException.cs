// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DuplicateDataInputException.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace RahulRai.Websites.Utilities.Common.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The duplicate data input exception.
    /// </summary>
    [Serializable]
    public class DuplicateDataInputException : BlogSystemException
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DuplicateDataInputException" /> class.
        /// </summary>
        public DuplicateDataInputException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDataInputException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public DuplicateDataInputException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDataInputException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public DuplicateDataInputException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateDataInputException"/> class.
        /// </summary>
        /// <param name="serializationInfo">
        /// The serialization info.
        /// </param>
        /// <param name="streamingContext">
        /// The streaming context.
        /// </param>
        protected DuplicateDataInputException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            // Add implementation.
        }

        #endregion
    }
}