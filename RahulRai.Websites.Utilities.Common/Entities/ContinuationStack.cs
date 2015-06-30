// ***********************************************************************
// Assembly         : RahulRai.Websites.Utilities.Common
// Author           : rahulrai
// Created          : 06-03-2015
//
// Last Modified By : rahulrai
// Last Modified On : 06-24-2015
// ***********************************************************************
// <copyright file="ContinuationStack.cs" company="Rahul Rai">
//     Copyright (c) Rahul Rai. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System;
    using System.Collections;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    /// <summary>
    ///     Class ContinuationStack.
    /// </summary>
    [Serializable]
    public class ContinuationStack
    {
        /// <summary>
        ///     The stack
        /// </summary>
        private readonly Stack stack;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContinuationStack" /> class.
        /// </summary>
        public ContinuationStack()
        {
            this.stack = new Stack();
        }

        /// <summary>
        ///     Determines whether this instance [can move back].
        /// </summary>
        /// <returns><c>true</c> if this instance [can move back]; otherwise, <c>false</c>.</returns>
        public bool CanMoveBack()
        {
            return this.stack.Count >= 2;
        }

        /// <summary>
        ///     Determines whether this instance [can move forward].
        /// </summary>
        /// <returns><c>true</c> if this instance [can move forward]; otherwise, <c>false</c>.</returns>
        public bool CanMoveForward()
        {
            return this.GetForwardToken() != null;
        }

        /// <summary>
        ///     Gets the back token.
        /// </summary>
        /// <returns>TableContinuationToken.</returns>
        public TableContinuationToken GetBackToken()
        {
            if (this.stack.Count == 0)
            {
                return null;
            }

            this.stack.Pop();
            this.stack.Pop();
            if (this.stack.Count == 0)
            {
                return null;
            }

            return this.stack.Peek() as TableContinuationToken;
        }

        /// <summary>
        ///     Gets the forward token.
        /// </summary>
        /// <returns>TableContinuationToken.</returns>
        public TableContinuationToken GetForwardToken()
        {
            if (this.stack.Count == 0)
            {
                return null;
            }

            return this.stack.Peek() as TableContinuationToken;
        }

        /// <summary>
        ///     Adds the token.
        /// </summary>
        /// <param name="result">The result.</param>
        public void AddToken(TableContinuationToken result)
        {
            this.stack.Push(result);
        }
    }
}