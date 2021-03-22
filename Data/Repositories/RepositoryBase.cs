
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

        public async Task Create(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }
        public Task Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task<IQueryable<T>> FindAll()
        {
            return Task.FromResult(_dbContext.Set<T>().AsNoTracking());
        }

        public Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(_dbContext.Set<T>().Where(expression).AsNoTracking());
        }


    }
}
