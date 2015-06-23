namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System.Collections;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    /// <summary>
    ///     Class ContinuationStack.
    /// </summary>
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
            stack = new Stack();
        }

        /// <summary>
        ///     Determines whether this instance [can move back].
        /// </summary>
        /// <returns><c>true</c> if this instance [can move back]; otherwise, <c>false</c>.</returns>
        public bool CanMoveBack()
        {
            return stack.Count >= 2;
        }

        /// <summary>
        ///     Determines whether this instance [can move forward].
        /// </summary>
        /// <returns><c>true</c> if this instance [can move forward]; otherwise, <c>false</c>.</returns>
        public bool CanMoveForward()
        {
            return GetForwardToken() != null;
        }

        /// <summary>
        ///     Gets the back token.
        /// </summary>
        /// <returns>TableContinuationToken.</returns>
        public TableContinuationToken GetBackToken()
        {
            if (stack.Count == 0)
            {
                return null;
            }

            stack.Pop();
            stack.Pop();
            if (stack.Count == 0)
            {
                return null;
            }

            return stack.Peek() as TableContinuationToken;
        }

        /// <summary>
        ///     Gets the forward token.
        /// </summary>
        /// <returns>TableContinuationToken.</returns>
        public TableContinuationToken GetForwardToken()
        {
            if (stack.Count == 0)
            {
                return null;
            }

            return stack.Peek() as TableContinuationToken;
        }

        /// <summary>
        ///     Adds the token.
        /// </summary>
        /// <param name="result">The result.</param>
        public void AddToken(TableContinuationToken result)
        {
            stack.Push(result);
        }
    }
}