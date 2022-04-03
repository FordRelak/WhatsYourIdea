using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WhatsYourIdea.Extenshion
{
    public static class CollectionExtenshion
    {
        public static IQueryable<Entity> IncludeAll<Entity>(this IQueryable<Entity> query,
                                                            params Expression<Func<Entity, object>>[] inludes) where Entity : class
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (inludes is null)
            {
                throw new ArgumentNullException(nameof(inludes));
            }

            return inludes.Aggregate(query, (ac, func) => ac.Include(func));
        }
    }
}