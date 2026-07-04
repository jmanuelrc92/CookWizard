using CookWizard.Application.Recipes.Mappings;
using CookWizard.Application.Users.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CookWizard.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        RecipeMappingConfig.RegisterMappings();
        UserMappingConfig.RegisterMappings();

        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}