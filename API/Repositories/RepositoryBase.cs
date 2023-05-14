using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly DataContext _context;
        public RepositoryBase(DataContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            if (trackChanges)
            {
                return _context.Set<T>();
            }
            else
            {
                return _context.Set<T>().AsNoTracking();
            }
        }

        public IQueryable<T> FindAllByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (trackChanges)
            {
                return _context.Set<T>().Where(expression);
            }
            else
            {
                return _context.Set<T>().Where(expression).AsNoTracking();
            }
        }

        public async Task<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (trackChanges)
            {
                return await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Set<T>().Where(expression).AsNoTracking().FirstOrDefaultAsync ();
            }
        }
    }
}