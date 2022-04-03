using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<Entity> where Entity : BaseEntity
    {
        Task<Entity> AddOrUpdateAsync(Entity entity);

        Task<Entity> GetAsync(Guid id);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<System.Linq.IQueryable<Entity>, IIncludableQueryable<Entity, object>> include);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<System.Linq.IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, Func<System.Linq.IQueryable<Entity>, System.Linq.IOrderedQueryable<Entity>> order);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<System.Linq.IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, Func<System.Linq.IQueryable<Entity>, System.Linq.IOrderedQueryable<Entity>> order, int pageNum);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<System.Linq.IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, Func<System.Linq.IQueryable<Entity>, System.Linq.IOrderedQueryable<Entity>> order, int pageNum, int pageSize);

        Task Remove(Entity entity);
    }
}