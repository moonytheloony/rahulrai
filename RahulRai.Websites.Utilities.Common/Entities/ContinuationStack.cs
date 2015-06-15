namespace RahulRai.Websites.Utilities.Common.Entities
{
    #region

    using System.Collections;
    using Microsoft.WindowsAzure.Storage.Table;

    #endregion

    public class ContinuationStack
    {
        private readonly Stack stack;

        public ContinuationStack()
        {
            stack = new Stack();
        }

        public bool CanMoveBack()
        {
            return stack.Count >= 2;
        }

        public bool CanMoveForward()
        {
            return GetForwardToken() != null;
        }

        public TableContinuationToken GetBackToken()
        {
            if (stack.Count == 0)
                return null;
            // need to pop twice and then return what is left
            stack.Pop();
            stack.Pop();
            if (stack.Count == 0)
            {
                return null;
            }

            return stack.Peek() as TableContinuationToken;
        }

        public TableContinuationToken GetForwardToken()
        {
            if (stack.Count == 0)
            {
                return null;
            }

            return stack.Peek() as TableContinuationToken;
        }

        public void AddToken(TableContinuationToken result)
        {
            stack.Push(result);
        }
    }
}