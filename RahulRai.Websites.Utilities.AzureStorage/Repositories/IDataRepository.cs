#region



#endregion

namespace RahulRai.Websites.Utilities.AzureStorage.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    #region

    

    #endregion

    /// <summary>
    ///     The DataRepository interface.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     Data Entity
    /// </typeparam>
    public interface IDataRepository<TEntity> : IDisposable
        where TEntity : class
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        void DeleteEntity(object id);

        /// <summary>
        ///     The delete.
        /// </summary>
        /// <param name="entityToDelete">
        ///     The entity to delete.
        /// </param>
        void DeleteEntity(TEntity entityToDelete);

        /// <summary>
        ///     The get.
        /// </summary>
        /// <param name="filter">
        ///     The filter.
        /// </param>
        /// <param name="orderBy">
        ///     The order by.
        /// </param>
        /// <param name="includeProperties">
        ///     The include properties.
        /// </param>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<TEntity> GetValues(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        ///     Creates the query.
        /// </summary>
        /// <returns>A query.</returns>
        IQueryable<TEntity> CreateQuery();

        /// <summary>
        ///     Gets the query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns>IQuery object of the query</returns>
        IQueryable<TEntity> GetQuery(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        ///     The get by id.
        /// </summary>
        /// <param name="id">
        ///     The id.
        /// </param>
        /// <returns>
        ///     The <see cref="TEntity" />.
        /// </returns>
        TEntity GetById(object id);

        /// <summary>
        ///     The insert.
        /// </summary>
        /// <param name="entity">
        ///     The entity.
        /// </param>
        void InsertEntity(TEntity entity);

        /// <summary>
        ///     The save.
        /// </summary>
        void Save();

        /// <summary>
        ///     The update.
        /// </summary>
        /// <param name="entityToUpdate">
        ///     The entity to update.
        /// </param>
        void UpdateEntity(TEntity entityToUpdate);

        #endregion
    }
}