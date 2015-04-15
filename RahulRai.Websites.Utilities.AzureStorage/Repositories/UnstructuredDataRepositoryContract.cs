#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    #region

    

    #endregion

    /// <summary>
    ///     The unstructured data repository contract.
    /// </summary>
    /// <typeparam name="TElement">
    ///     Element to store in storage.
    /// </typeparam>
    [ContractClassFor(typeof (IUnstructuredDataRepository<>))]
    public abstract class UnstructuredDataRepositoryContract<TElement> : IUnstructuredDataRepository<TElement>
        where TElement : class, new()
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The create storage object and set execution context.
        /// </summary>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void CreateStorageObjectAndSetExecutionContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void Delete(TElement entity)
        {
            Contract.Requires<InputValidationFailedException>(null != entity, "entity");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The delete storage object.
        /// </summary>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void DeleteStorageObject()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <returns>
        ///     The <see cref="IList" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public IList<TElement> GetAll(string key)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrWhiteSpace(key), "key");
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IList<TElement> GetAll()
        {
            throw new NotImplementedException();
        }

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
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public TElement GetById(string key, string id)
        {
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrWhiteSpace(key), "key");
            Contract.Requires<InputValidationFailedException>(!string.IsNullOrWhiteSpace(id), "id");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The insert.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void Insert(TElement entity)
        {
            Contract.Requires<InputValidationFailedException>(null != entity, "entity");
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void InsertOrReplace(TElement entity)
        {
            Contract.Requires<InputValidationFailedException>(null != entity, "entity");
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The merge.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void Merge(TElement entity)
        {
            Contract.Requires<InputValidationFailedException>(null != entity, "entity");
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IList<TElement> Query(string filter, int? takeCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The save.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public bool Save()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IList<OperationResult> SaveAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The set execution context.
        /// </summary>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void SetExecutionContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        /// <exception cref="NotImplementedException">
        ///     Not implemented in contract class.
        /// </exception>
        public void Update(TElement entity)
        {
            Contract.Requires<InputValidationFailedException>(null != entity, "entity");
            throw new NotImplementedException();
        }

        #endregion
    }
}