
using ActivityTrackerApi.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public RepositoryBase(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public Task<IQueryable<T>> FindAll()
        {
            return Task.Run(() => _dbContext.Set<T>().AsNoTracking());
        }

        public Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {   
            return Task.Run(() => _dbContext.Set<T>().Where(expression).AsNoTracking());
        }


    }
}
