#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    #region

    

    #endregion

    /// <summary>
    ///     The UnstructuredDataRepository interface.
    /// </summary>
    /// <typeparam name="TElement">
    ///     Element for table storage
    /// </typeparam>
    [ContractClass(typeof (UnstructuredDataRepositoryContract<>))]
    public interface IUnstructuredDataRepository<TElement>
        where TElement : class, new()
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The create storage object and set execution context.
        /// </summary>
        void CreateStorageObjectAndSetExecutionContext();

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        void Delete(TElement entity);

        /// <summary>
        ///     The delete storage object.
        /// </summary>
        void DeleteStorageObject();

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     The <see cref="IList" />.
        /// </returns>
        IList<TElement> GetAll(string key);

        /// <summary>
        ///     Gets all items in the table.
        /// </summary>
        /// <returns>All items in the table.</returns>
        IList<TElement> GetAll();

        /// <summary>
        ///     The get by id.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="TElement" />.
        /// </returns>
        TElement GetById(string key, string id);

        /// <summary>
        ///     The insert.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        void Insert(TElement entity);

        /// <summary>
        ///     Inserts or replaces the entity.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        void InsertOrReplace(TElement entity);

        /// <summary>
        ///     The merge.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        void Merge(TElement entity);

        /// <summary>
        ///     Queries the table using specified filter string.
        /// </summary>
        /// <param name="filter">
        ///     The filter string. Use <see cref="TableQuery" /> to construct the filter.
        /// </param>
        /// <param name="takeCount">
        ///     The take count.
        /// </param>
        /// <returns>
        ///     The query.
        /// </returns>
        IList<TElement> Query(string filter, int? takeCount);

        /// <summary>
        ///     The save.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        bool Save();

        /// <summary>
        ///     Saves all operations as batch.
        /// </summary>
        /// <returns>List of results for each item.</returns>
        IList<OperationResult> SaveAll();

        /// <summary>
        ///     The set execution context.
        /// </summary>
        void SetExecutionContext();

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        void Update(TElement entity);

        #endregion
    }
}