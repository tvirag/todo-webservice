using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Todo.Interfaces
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetByID(object id);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);

    }
}
