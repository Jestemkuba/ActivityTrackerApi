﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Data.Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        Task<IQueryable<T>> FindAll();
        Task<IQueryable<T>> FindByCondition(Expression<Func<T, bool>> expression);
        Task Create(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
