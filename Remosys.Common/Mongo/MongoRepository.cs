using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Remosys.Common.Helper.Pagination;
using Remosys.Common.Types;

namespace Remosys.Common.Mongo
{
    public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IIdentifiable
    {
        protected IMongoCollection<TEntity> Collection { get; }

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
            Collection = database.GetCollection<TEntity>(collectionName);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
            => Collection.AsQueryable().Where(predicate);

        public IQueryable<TEntity> GetAll() => Collection.AsQueryable();



        public async Task<TEntity> GetAsync(Guid id, CancellationToken cancellationToken)
            => await GetAsync(e => e.Id == id, cancellationToken);

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken) => await Collection.Find(predicate).SingleOrDefaultAsync(cancellationToken);

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken) => await Collection.Find(predicate).ToListAsync(cancellationToken);

        public async Task<PagedResult<TEntity>> BrowseAsync(IQueryable<TEntity> query, PagingOptions pagingOptions)
        {
            var mongoQuery = (IMongoQueryable<TEntity>)query;

            return await mongoQuery.PaginateAsync(pagingOptions);
        }


        public async Task<PagedResult<TEntity>> BrowseAsync(Expression<Func<TEntity, bool>> predicate,
            PagingOptions pagingOptions)
            => await Collection.AsQueryable().Where(predicate).PaginateAsync(pagingOptions);

        public async Task<PagedResult<TEntity>> BrowseAsync<TQuery>(Expression<Func<TEntity, bool>> predicate,
            TQuery query) where TQuery : PagedQueryBase
            => await Collection.AsQueryable().Where(predicate).PaginateAsync(query);

        public async Task AddAsync(TEntity entity) => await Collection.InsertOneAsync(entity);
        public async Task AddRangeAsync(List<TEntity> entities)
            => await Collection.InsertManyAsync(entities);


        public async Task UpdateAsync(TEntity entity) =>
            await Collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

    


        public async Task DeleteAsync(Guid id)
            => await Collection.DeleteOneAsync(e => e.Id == id);

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken) => await Collection.Find(predicate).AnyAsync(cancellationToken);


    }
}