using CookWizard.Domain.Auth.Repository;
using CookWizard.Domain.Auth.Services;
using CookWizard.Domain.Users.Repository;
using CookWizard.Infrastructure.Auth.Persistance;
using CookWizard.Infrastructure.Auth.Services;
using CookWizard.Infrastructure.Common.Settings;
using CookWizard.Infrastructure.Users.Persistance;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace CookWizard.Infraestructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOptions<JWTSettings>()
            .Bind(configuration.GetSection("JWT"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        _ConfigureMongoDbSerialization();

        services.AddOptions<MongoSettings>()
            .Bind(configuration.GetSection("MongoDB"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoSettings>>().Value;

            string connectionUri = $"mongodb+srv://{options.User}:{options.Password}@{options.Server}/?appName={options.AppName}";

            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            return new MongoClient(settings);
        });

        //services.AddScoped<IRecipeRepository, MongoRecipeRepository>();
        services.AddScoped<IUserRepository, MongoUserRepository>();
        services.AddScoped<IUserCredentialsRepository, MongoUserCredentialsRepository>();

        services.AddScoped<IPasswordHasherService, Argon2HasherService>();
        services.AddScoped<IJWTService, JwtService>();

        return services;
    }

    private static void _ConfigureMongoDbSerialization()
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