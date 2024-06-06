using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using BookCatalogApi.Attributes;

namespace BookCatalogApi.Models
{
    [MongoDbCollectionName(name: "books")]
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; } = string.Empty;

        [BsonElement("description")]
        [BsonRepresentation(BsonType.String)]
        public string Description { get; set; } = string.Empty;

        [BsonElement("author")]
        [BsonRepresentation(BsonType.String)]
        public string Author { get; set; } = string.Empty;

        [BsonElement("price")]
        [BsonRepresentation(BsonType.Double)]
        public double Price { get; set; }

        [BsonElement("bookCoverPath")]
        [BsonRepresentation(BsonType.String)]
        public string? BookCoverPath { get; set; }

        [BsonElement("rating")]
        [BsonRepresentation(BsonType.Int32)]
        public int Rating { get; set; }
    }
}
