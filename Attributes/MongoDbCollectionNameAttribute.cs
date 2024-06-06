namespace BookCatalogApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDbCollectionNameAttribute(string name) : Attribute
    {
        public string Name { get; set; } = name;
    }
}
