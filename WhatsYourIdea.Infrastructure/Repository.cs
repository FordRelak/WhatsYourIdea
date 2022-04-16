using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace WhatsYourIdea.Infrastructure
{
    public sealed class Repository<Entity> : IRepository<Entity> where Entity : BaseEntity
    {
        private const int DEFAULT_PAGE_SIZE = 20;

        private readonly EfDbContext _context;
        private readonly DbSet<Entity> _db;

        public Repository(EfDbContext context)
        {
            _context = context;
            _db = context.Set<Entity>();
        }

        private static IQueryable<Entity> GetQuery(IQueryable<Entity> query,
                                            Expression<Func<Entity, bool>> predicate,
                                            Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include,
                                            Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order)
        {
            query = query.AsNoTracking();

            if(predicate is not null)
            {
                query = query.Where(predicate);
            }

            if(include is not null)
            {
                query = include(query);
            }

            if(order is not null)
            {
                query = order(query);
            }

            return query;
        }

        public async Task<Entity> GetAsync(int id)
        {
            return await _db.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate)
        {
            return await GetAsync(predicate, null, null, 1, DEFAULT_PAGE_SIZE);
        }

        public async Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate,
                                                        Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include)
        {
            return await GetAsync(predicate, include, null, 1, DEFAULT_PAGE_SIZE);
        }

        public async Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate,
                                                        Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include,
                                                        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order)
        {
            return await GetAsync(predicate, include, order, 1, DEFAULT_PAGE_SIZE);
        }

        public async Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate,
                                                        Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include,
                                                        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order,
                                                        int pageNum)
        {
            return await GetAsync(predicate, include, order, pageNum, DEFAULT_PAGE_SIZE);
        }

        public async Task<IEnumerable<Entity>> GetAsync(Expression<Func<Entity, bool>> predicate,
                                                        Func<IQueryable<Entity>, IIncludableQueryable<Entity, object>> include,
                                                        Func<IQueryable<Entity>, IOrderedQueryable<Entity>> order,
                                                        int pageNum,
                                                        int pageSize)
        {
            var query = GetQuery(_db.AsQueryable(), predicate, include, order);
            query = query.Skip(pageSize * (pageNum < 1 ? 1 : pageNum)).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<Entity> AddOrUpdateAsync(Entity entity)
        {
            if(entity.Id == default)
                _context.Add(entity);
            else
                _context.Update(entity);

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Remove(Entity entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}