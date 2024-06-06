using MongoDB.Driver;
using BookCatalogApi.Attributes;

namespace BookCatalogApi.Infrastructure
{
    public abstract class RepositoryBase<TEntity> where TEntity : class, new()
    {
        private readonly MongoDbContext _dbContext;
        private readonly IMongoCollection<TEntity> _dbCollection;

        public RepositoryBase(MongoDbContext dbContext)
        {
            var attribute = typeof(TEntity)
                .GetCustomAttributes(typeof(MongoDbCollectionNameAttribute), inherit: true)
                .OfType<MongoDbCollectionNameAttribute>()
                .FirstOrDefault();

            _dbContext = dbContext;
            _dbCollection = _dbContext.GetCollection<TEntity>(attribute?.Name ?? string.Empty);
        }

        protected IMongoCollection<TEntity> DbCollection => _dbCollection;
    }
}
