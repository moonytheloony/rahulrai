#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Diagnostics.Contracts;

    #region

    

    #endregion

    /// <summary>
    ///     The Context interface.
    /// </summary>
    [ContractClass(typeof (ContextContract))]
    public interface IContext : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the database connection string.
        /// </summary>
        string DatabaseConnectionString { get; set; }

        /// <summary>
        ///     Gets the file repository.
        /// </summary>
        IFileRepository FileRepository { get; }

        /// <summary>
        ///     Gets or sets the file server access string.
        /// </summary>
        string FileServerAccessString { get; set; }

        /// <summary>
        ///     Gets or sets the messaging  settings.
        /// </summary>
        QueueSettings MessagingSettings { get; set; }

        /// <summary>
        ///     Gets or sets the unstructured storage connection string.
        /// </summary>
        string UnstructuredStorageConnectionString { get; set; }

        /// <summary>
        ///     Gets or sets the unstructured storage object name.
        /// </summary>
        string UnstructuredStorageObjectName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The get identity repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     The data entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDataRepository{TDataEntity}" />.
        /// </returns>
        IDataRepository<TDataEntity> GetIdentityRepository<TDataEntity>() where TDataEntity : class;

        /// <summary>
        ///     The get message repository.
        /// </summary>
        /// <typeparam name="TMessageEntity">
        ///     Message Entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IMessageRepository{TDataEntity}" />.
        /// </returns>
        IMessageRepository<TMessageEntity> GetMessageRepository<TMessageEntity>() where TMessageEntity : class;

        /// <summary>
        ///     The get data repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Data Entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDataRepository{TDataEntity}" />.
        /// </returns>
        IDataRepository<TDataEntity> GetMetadataRepository<TDataEntity>() where TDataEntity : class;

        /// <summary>
        ///     The get unstructured data repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Type of entity to store.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IUnstructuredDataRepository{TDataEntity}" />.
        /// </returns>
        IUnstructuredDataRepository<TDataEntity> GetUnstructuredDataRepository<TDataEntity>()
            where TDataEntity : class, new();

        /// <summary>
        ///     The get unstructured data repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Type of entity to store.
        /// </typeparam>
        /// <param name="convertToTableEntity">
        ///     The converter from entity to table entity.
        /// </param>
        /// <param name="convertToEntity">
        ///     The converter from table entity to entity.
        /// </param>
        /// <returns>
        ///     The <see cref="IUnstructuredDataRepository{TDataEntity}" />.
        /// </returns>
        IUnstructuredDataRepository<TDataEntity> GetUnstructuredDataRepository<TDataEntity>(
            Func<TDataEntity, DynamicTableEntity> convertToTableEntity,
            Func<DynamicTableEntity, TDataEntity> convertToEntity)
            where TDataEntity : class, new();

        /// <summary>
        ///     The get workflow repository.
        /// </summary>
        /// <typeparam name="TDataEntity">
        ///     Data Entity
        /// </typeparam>
        /// <returns>
        ///     The <see cref="IDataRepository{TDataEntity}" />.
        ///     Data Repository
        /// </returns>
        IDataRepository<TDataEntity> GetWorkflowRepository<TDataEntity>() where TDataEntity : class;

        #endregion
    }
}