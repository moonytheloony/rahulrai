#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Diagnostics.Contracts;

    #region

    

    #endregion

    /// <summary>
    ///     The context contract.
    /// </summary>
    [ContractClassFor(typeof (IContext))]
    public abstract class ContextContract : IContext
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the database connection string.
        /// </summary>
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        ///     Gets the file repository.
        /// </summary>
        /// <exception cref="NotImplementedException">
        ///     throws exception
        /// </exception>
        public IFileRepository FileRepository
        {
            get
            {
                Contract.Requires<InputValidationFailedException>(
                    !string.IsNullOrWhiteSpace(FileServerAccessString),
                    "FileServerAccessString");
                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Gets or sets the file server access string.
        /// </summary>
        public string FileServerAccessString { get; set; }

        /// <summary>
        ///     Gets or sets the messaging  settings.
        /// </summary>
        public QueueSettings MessagingSettings { get; set; }

        /// <summary>
        ///     Gets or sets the unstructured storage connection string.
        /// </summary>
        public string UnstructuredStorageConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the unstructured storage object name.
        /// </summary>
        public string UnstructuredStorageObjectName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException">Not implemented Exception</exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The get identity repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Data entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDataRepository" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Contract not implemented
        /// </exception>
        public IDataRepository<TDataEntity> GetIdentityRepository<TDataEntity>() where TDataEntity : class
        {
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(DatabaseConnectionString),
                "DatabaseConnectionString");
            throw new NotImplementedException("Contract");
        }

        /// <summary>
        ///     The get message repository.
        /// </summary>
        /// <typeparam name="TMessageEntity">
        ///     Message Entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IMessageRepository" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented
        /// </exception>
        public IMessageRepository<TMessageEntity> GetMessageRepository<TMessageEntity>() where TMessageEntity : class
        {
            Contract.Requires<InputValidationFailedException>(
                null != MessagingSettings,
                "MessagingSystemAccessString");
            Contract.Requires<InputValidationFailedException>(
                null != MessagingSettings.ServiceQueueEndpoint,
                "MessagingSystemAccessString");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The get metadata repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Data Entity
        /// </typeparam>
        /// <returns>
        ///     The
        ///     <see>
        ///         <cref>IDataRepository</cref>
        ///     </see>
        ///     .
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented
        /// </exception>
        public IDataRepository<TDataEntity> GetMetadataRepository<TDataEntity>() where TDataEntity : class
        {
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(DatabaseConnectionString),
                "DatabaseConnectionString");
            throw new NotImplementedException("Contract");
        }

        /// <summary>
        ///     The get unstructured data repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Type of entity to store.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IUnstructuredDataRepository" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented by contract
        /// </exception>
        public IUnstructuredDataRepository<TDataEntity> GetUnstructuredDataRepository<TDataEntity>()
            where TDataEntity : class, new()
        {
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(UnstructuredStorageConnectionString),
                "storage connection string");
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(UnstructuredStorageObjectName),
                "storage object name");
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IUnstructuredDataRepository<TDataEntity> GetUnstructuredDataRepository<TDataEntity>(
            Func<TDataEntity, DynamicTableEntity> convertToTableEntity,
            Func<DynamicTableEntity, TDataEntity> convertToEntity) where TDataEntity : class, new()
        {
            Contract.Requires<InputValidationFailedException>(convertToTableEntity != null, "convertToTableEntity");
            Contract.Requires<InputValidationFailedException>(convertToEntity != null, "convertToEntity");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The get workflow repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Data entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDataRepository" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented
        /// </exception>
        public IDataRepository<TDataEntity> GetWorkflowRepository<TDataEntity>() where TDataEntity : class
        {
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(DatabaseConnectionString),
                "DatabaseConnectionString");
            throw new NotImplementedException("Contract");
        }

        /// <summary>
        ///     The get transient metadata repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Data Entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDataRepository" />.
        ///     Data Repository
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented
        /// </exception>
        public IDataRepository<TDataEntity> GetTransientStoreRepository<TDataEntity>() where TDataEntity : class
        {
            Contract.Requires<InputValidationFailedException>(
                !string.IsNullOrWhiteSpace(DatabaseConnectionString),
                "DatabaseConnectionString");
            throw new NotImplementedException("Contract");
        }

        #endregion
    }
}