using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WebArchitecture.Data
{
    /// <summary>
    /// This class contains basic CRUD operations for entities of the DataModel
    /// </summary>
    /// <typeparam name="TEntity">Entity class</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal DbContext _context;
        internal DbSet<TEntity> _entities;

        public Repository(DbContext ctx)
        {
            _context = ctx;
            _entities = _context.Set<TEntity>();
        }

        /// <summary>
        /// Get method uses lambda expressions to allow the calling code to specify a filter condition and a column to order the results by, and a string parameter lets the caller provide a comma-delimited list of navigation properties for eager loading.
        /// NOTE: This method ensures that the filtering and sorting will be done by the database server instead of using web server memory.
        /// </summary>
        /// <param name="filter">Lambda expression based on the TEntity type; this expression should return a boolean value. (ex. student => student.LastName == "Smith")</param>
        /// <param name="orderBy">Lambda expression where the input to the expression is an IQueryable object for the TEntity type. The expression will return an ordered version of that IQueryable object. (ex. q => q.OrderBy(s => s.LastName))</param>
        /// <param name="includeProperties">Comma delimited list of properties that should be eager-loaded.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _entities;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        /// <summary>
        /// Search for an entity of the TEntity type using the specified id
        /// </summary>
        public virtual TEntity GetById(object id)
        {
            return _entities.Find(id);
        }

        /// <summary>
        /// Add the specified entity to the context.  The entity is not persisted until Save method is called.
        /// </summary>
        public virtual void Insert(TEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Add collection of entities to the context.
        /// </summary>
        /// <param name="entities"></param>
        public void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                Insert(entity);
            }
        }

        /// <summary>
        /// Search for an entity based on the specified id and remove the entity from the context using Delete(TEntity) method.
        /// </summary>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _entities.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Removes specified entity from the context.
        /// </summary>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == (System.Data.Entity.EntityState) EntityState.Detached)
            {
                _entities.Attach(entityToDelete);
            }
            _entities.Remove(entityToDelete);
        }

        /// <summary>
        /// Add entity to the context and marks it as Modified.
        /// </summary>
        public virtual void Update(TEntity entityToUpdate)
        {
            _entities.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = (System.Data.Entity.EntityState) EntityState.Modified;
        }

        /// <summary>
        /// Update existing entity with values from updated entity
        /// </summary>
        public virtual void Update(TEntity entityWithNewValues, TEntity existingEntity)
        {
            _context.Entry(existingEntity).CurrentValues.SetValues(entityWithNewValues);
        }

        /// <summary>
        /// Save changes
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
