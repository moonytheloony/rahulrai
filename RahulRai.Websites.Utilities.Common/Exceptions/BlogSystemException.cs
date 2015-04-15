﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CTPECSystemException.cs" company="Microsoft">
//   Copyright (c) Glasgow City Council. All Rights Reserved.
// </copyright>
// <summary>
//   The ctpec system exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RahulRai.Websites.Utilities.Common.Exceptions
{
    #region

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     The  system exception.
    /// </summary>
    [Serializable]
    public class BlogSystemException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogSystemException"/> class.
        /// </summary>
        public BlogSystemException()
        {
            // No implementation.           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogSystemException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public BlogSystemException(string message)
            : base(message)
        {
            // No implementation.            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogSystemException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public BlogSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
            // No implementation.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogSystemException"/> class.
        /// </summary>
        /// <param name="serializationInfo">
        /// The serialization info.
        /// </param>
        /// <param name="streamingContext">
        /// The streaming context.
        /// </param>
        protected BlogSystemException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            // Add implementation.
        }

        #endregion
    }
}