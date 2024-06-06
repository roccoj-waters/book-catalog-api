using EnsureThat;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using BookCatalogApi.Configuration;

namespace BookCatalogApi.Infrastructure
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly MongoClient _mongoClient;

        public MongoDbContext(IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            var mongoConnectionUrl = new MongoUrl(mongoDbConfiguration.Value.ConnectionUrl);
            var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
            _mongoClient = new MongoClient(mongoClientSettings);
            _mongoDatabase = _mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _mongoDatabase.GetCollection<T>(name);
        }

        public void DropCollection(string name)
        {
            _mongoDatabase.DropCollection(name);
        }

        public bool ImportRecords(string collectionName, string jsonContent)
        {
            EnsureArg.IsNotNullOrWhiteSpace(collectionName, nameof(collectionName));
            EnsureArg.IsNotNullOrWhiteSpace(jsonContent, nameof(jsonContent));
            IEnumerable<BsonDocument> documents = BsonSerializer.Deserialize<IEnumerable<BsonDocument>>(jsonContent);
            GetCollection<BsonDocument>(collectionName).InsertMany(documents);
            return true;
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}
