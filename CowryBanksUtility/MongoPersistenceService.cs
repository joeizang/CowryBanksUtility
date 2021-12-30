using MongoDB.Driver;

namespace CowryBanksUtility
{
    public interface IMongodbService
    {
        Task CreateOneAsync(Bank entity);

        string CollectionName { get; set; }
    }

    public class MongoDatabaseSettings : IMongoDatabaseSettings
    {
        public string CollectionName { get; set; } = "CowryBanks";
        public string DatabaseName { get; set; } = "SLACowryWiseDb";
        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
    }

    public interface IMongoDatabaseSettings
    {
        string CollectionName { get; set; }

        string DatabaseName { get; set; }

        string ConnectionString { get; set; }
    }
    public class MongodbPersistenceService : IMongodbService
    {
        private readonly IMongoCollection<Bank> _genericCollection;

        public string CollectionName { get; set; }
        public MongodbPersistenceService(IMongoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.DatabaseName);
            _genericCollection = db.GetCollection<Bank>("CowryBanks");
        }
        public async Task CreateOneAsync(Bank entity)
        {
            await _genericCollection.InsertOneAsync(entity).ConfigureAwait(false);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
