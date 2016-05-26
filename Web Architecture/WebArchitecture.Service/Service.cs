using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebArchitecture.Data;

namespace WebArchitecture.Service
{
    public abstract class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        protected Service(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual void Delete(TEntity entity)
        {
            _repository.Delete(entity);
        }

        public virtual void Delete(object id)
        {
            _repository.Delete(id);
        }

        public virtual TEntity Find(string id)
        {
            return _repository.GetById(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _repository.Insert(entity);
        }

        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            _repository.InsertRange(entities);
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return _repository.Get(query).AsQueryable();
        }

        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }
    }
}
