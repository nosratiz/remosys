using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Types;

namespace Remosys.Common.Mongo
{
    public interface IMongoRepository<TEntity> where TEntity : IIdentifiable
    {
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> GetAll();
        Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken);

        Task<PagedResult<TEntity>> BrowseAsync(IQueryable<TEntity> query, PagingOptions pagingOptions);

        Task<PagedResult<TEntity>> BrowseAsync(Expression<Func<TEntity, bool>> predicate, PagingOptions pagingOptions);

        Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate, TQuery query)
            where TQuery : PagedQueryBase;

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    }
}