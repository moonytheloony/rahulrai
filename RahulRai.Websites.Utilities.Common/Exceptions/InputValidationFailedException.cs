// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 04-15-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="InputValidationFailedException.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Exceptions
{
    #region

    using System;
    using System.Runtime.Serialization;

    #endregion

    /// <summary>
    ///     The input validation failed exception.
    /// </summary>
    [Serializable]
    public class InputValidationFailedException : BlogSystemException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InputValidationFailedException" /> class.
        /// </summary>
        public InputValidationFailedException()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputValidationFailedException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InputValidationFailedException(string message)
            : base(message)
        {
            //// Implemented through handler
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputValidationFailedException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InputValidationFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
            //// Implemented through handler
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputValidationFailedException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected InputValidationFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            // Add implementation.
        }
    }
}