using CookWizard.Domain.Interfaces;
using CookWizard.Infraestructure.Persistance;
using CookWizard.Infrastructure.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace CookWizard.Infraestructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureMongoDbSerialization();

        services.AddOptions<MongoSettings>()
            .Bind(configuration.GetSection("MongoDB"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IRecipeRepository, MongoRecipeRepository>();

        return services;
    }

    private static void ConfigureMongoDbSerialization()
    {
        // every Guid in Mongo stored as string.
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

        // CamalCase use convention
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true)
        };

        ConventionRegistry.Register("CookWizardConventions", conventionPack, t => true);
    }
}