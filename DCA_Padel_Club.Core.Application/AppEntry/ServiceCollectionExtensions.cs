using System.Reflection;
using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using Microsoft.Extensions.DependencyInjection;

namespace DCA_Padel_Club.Core.Application.AppEntry;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlersFromAssembly(
        this IServiceCollection services,
        Assembly assembly
    )
    {
        var handlerTypes = assembly
            .GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Where(t =>
                t.GetInterfaces()
                    .Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                    )
            );

        foreach (var handlerType in handlerTypes)
        {
            var handlerInterface = handlerType
                .GetInterfaces()
                .First(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)
                );

            // Register: ICommandHandler<TCommand> → ConcreteHandler
            services.AddScoped(handlerInterface, handlerType);
        }

        return services;
    }
}
