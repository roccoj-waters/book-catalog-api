using MongoDB.Driver;
using BookCatalogApi.Models;

namespace BookCatalogApi.Infrastructure
{
    public class BookRepository(MongoDbContext dbContext)
        : RepositoryBase<Book>(dbContext)
    {
        public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken)
            => await DbCollection.Find(_ => true).ToListAsync(cancellationToken);

        public async Task<Book> GetByIdAsync(string id, CancellationToken cancellationToken)
            => await DbCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);

        public async Task CreateAsync(Book newBook, CancellationToken cancellationToken)
            => await DbCollection.InsertOneAsync(newBook, new InsertOneOptions { }, cancellationToken);

        public async Task UpdateAsync(string id, Book updatedBook, CancellationToken cancellationToken)
            => await DbCollection.ReplaceOneAsync(x => x.Id == id, updatedBook, new ReplaceOptions { }, cancellationToken);

        public async Task RemoveAsync(string id, CancellationToken cancellationToken)
            => await DbCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }
}
