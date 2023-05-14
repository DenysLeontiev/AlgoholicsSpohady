using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}