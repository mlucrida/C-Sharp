using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebArchitecture.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get method uses lambda expressions to allow the calling code to specify a filter condition and a column to order the results by, and a string parameter lets the caller provide a comma-delimited list of navigation properties for eager loading.
        /// NOTE: This method ensures that the filtering and sorting will be done by the database server instead of using web server memory.
        /// </summary>
        /// <param name="filter">Lambda expression based on the TEntity type; this expression should return a boolean value. (ex. student => student.LastName == "Smith")</param>
        /// <param name="orderBy">Lambda expression where the input to the expression is an IQueryable object for the TEntity type. The expression will return an ordered version of that IQueryable object. (ex. q => q.OrderBy(s => s.LastName))</param>
        /// <param name="includeProperties">Comma delimited list of properties that should be eager-loaded.</param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// Search for an entity of the TEntity type using the specified id
        /// </summary>
        TEntity GetById(object id);

        /// <summary>
        /// Add the specified entity to the context.  The entity is not persisted until Save method is called.
        /// </summary>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert a collection of entities
        /// </summary>
        /// <param name="entities"></param>
        void InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Search for an entity based on the specified id and remove the entity from the context using Delete(TEntity) method.
        /// </summary>
        void Delete(object id);

        /// <summary>
        /// Removes specified entity from the context.
        /// </summary>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Add entity to the context and marks it as Modified.
        /// </summary>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Update existing entity with values from updated entity
        /// </summary>
        void Update(TEntity entityWithNewValues, TEntity existingEntity);

        /// <summary>
        /// Save changes
        /// </summary>
        void Save();
    }
}