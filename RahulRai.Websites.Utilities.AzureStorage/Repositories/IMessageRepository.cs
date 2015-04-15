#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Collections.Generic;

    #region

    

    #endregion

    /// <summary>
    ///     The MessageRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     Message Entity
    /// </typeparam>
    public interface IMessageRepository<TEntity>
        where TEntity : class
    {
        #region Public Events

        /// <summary>
        ///     The on error received.
        /// </summary>
        event EventHandler<Exception> OnErrorReceived;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The abandon message.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     message if null
        /// </exception>
        bool AbandonMessage(QueueMessage<TEntity> message);

        /// <summary>
        ///     The add message.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="enqueueTime">
        ///     The enqueue time.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     message | enqueue time
        /// </exception>
        void AddMessage(QueueMessage<TEntity> message, DateTime enqueueTime);

        /// <summary>
        ///     Creates the queue if it does not exist.
        /// </summary>
        void CreateQueueIfNotExists();

        /// <summary>
        ///     Deletes the queue.
        /// </summary>
        void DeleteQueue();

        /// <summary>
        ///     Removes a message from the queue to mark it as successfully processed.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <exception cref="InputValidationFailedException">
        ///     message if null
        /// </exception>
        void DequeueMessage(QueueMessage<TEntity> message);

        /// <summary>
        ///     Removes a batch of messages from the queue to mark them as successfully processed.
        /// </summary>
        /// <param name="messages">
        ///     The message to de queue.
        /// </param>
        /// <exception cref="InputValidationFailedException">
        ///     If messages is <c>null</c>.
        /// </exception>
        void DequeueMessageBatch(IEnumerable<QueueMessage<TEntity>> messages);

        /// <summary>
        ///     The get message.
        /// </summary>
        /// <param name="messageCount">
        ///     The message count.
        /// </param>
        /// <param name="waitTime">
        ///     The wait time.
        /// </param>
        /// <returns>
        ///     The Messages to return.
        /// </returns>
        List<QueueMessage<TEntity>> GetMessage(int messageCount, TimeSpan waitTime);

        /// <summary>
        ///     The register not synchronous receive.
        /// </summary>
        /// <param name="argument">
        ///     The argument.
        /// </param>
        void RegisterAsynchronousReceive(Action<MessagingEventArguments<TEntity>> argument);

        /// <summary>
        ///     The renew lease.
        /// </summary>
        /// <param name="message">
        ///     The message.
        /// </param>
        void RenewLease(QueueMessage<TEntity> message);

        #endregion
    }
}