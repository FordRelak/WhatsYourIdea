using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<Entity> where Entity : BaseEntity
    {
        Task<Entity> AddOrUpdateAsync(Entity entity);
        IEnumerable<Entity> Get();
        Task<Entity> GetAsync(int id);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order, int pageNum);

        Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate, Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include, Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order, int pageNum, int pageSize);
        Task<Entity> GetOneAsync(Expression<Func<Entity, bool>> predicate, Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include = null);
        Task Remove(Entity entity);
    }
}