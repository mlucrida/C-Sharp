using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebArchitecture.Service
{
    public interface IService<TEntity> where TEntity : class
    {
        TEntity Find(string id);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query);
    }
}
