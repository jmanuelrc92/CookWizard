using CookWizard.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CookWizard.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IMediatorCustom, MediatorCustom>();
        var assembly = Assembly.GetExecutingAssembly();

        var handlers = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandlerCustom<,>)));

        foreach (var handler in handlers)
        {
            var interfaceType = handler.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IRequestHandlerCustom<,>));
            services.AddScoped(interfaceType, handler);
        }

        return services;
    }
}