using CookWizard.Application.Common.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CookWizard.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        RecipeMappingConfig.RegisterMappings();

        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}