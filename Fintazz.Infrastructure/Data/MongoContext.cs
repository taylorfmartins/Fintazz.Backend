namespace Fintazz.Infrastructure.Data;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

public class MongoContext
{
    private readonly IMongoDatabase _database;
    private static bool _conventionsConfigured;

    public MongoContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
        
        ConfigureConventions();
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }

    private static void ConfigureConventions()
    {
        if (_conventionsConfigured) return;

        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new EnumRepresentationConvention(BsonType.String),
            new IgnoreExtraElementsConvention(true)
        };
        ConventionRegistry.Register("CustomConventions", pack, t => true);

        // Suporte a Guid string representation globalmente usando o padrão atual do Driver
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        
        _conventionsConfigured = true;
    }
}
