using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.PlayerCommands;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Application.Features.PlayerFeatures;
using DCA_Padel_Club.Core.Application.Features;
using DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Dispatcher;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCommandHandlersFromAssembly_RegistersPlayerHandlers()
    {
        
        IServiceCollection services = new ServiceCollection();
        var applicationAssembly = typeof(CreatePlayerHandler).Assembly;

        
        services.AddCommandHandlersFromAssembly(applicationAssembly);

        // Assert check registrations exist in the service collection (no construction needed)
        Assert.Contains(services, s =>
            s.ServiceType == typeof(ICommandHandler<CreatePlayerCommand>) &&
            s.ImplementationType == typeof(CreatePlayerHandler));

        Assert.Contains(services, s =>
            s.ServiceType == typeof(ICommandHandler<ManagePlayerStatusCommand>) &&
            s.ImplementationType == typeof(ManagePlayerStatusHandler));
    }

    [Fact]
    public void AddCommandHandlersFromAssembly_RegistersScheduleHandlers()
    {
        
        IServiceCollection services = new ServiceCollection();
        var applicationAssembly = typeof(CreateScheduleHandler).Assembly;

        
        services.AddCommandHandlersFromAssembly(applicationAssembly);

        
        Assert.Contains(services, s =>
            s.ServiceType == typeof(ICommandHandler<CreateScheduleCommand>) &&
            s.ImplementationType == typeof(CreateScheduleHandler));

        Assert.Contains(services, s =>
            s.ServiceType == typeof(ICommandHandler<ActivateScheduleCommand>) &&
            s.ImplementationType == typeof(ActivateScheduleHandler));

        Assert.Contains(services, s =>
            s.ServiceType == typeof(ICommandHandler<DeleteScheduleCommand>) &&
            s.ImplementationType == typeof(DeleteScheduleHandler));
    }

    [Fact]
    public void AddCommandHandlersFromAssembly_DoesNotRegisterNonHandlerClasses()
    {
        
        IServiceCollection services = new ServiceCollection();
        var applicationAssembly = typeof(CreatePlayerHandler).Assembly;

        
        services.AddCommandHandlersFromAssembly(applicationAssembly);

        // Assert DummyCommandA has no handler in the Application assembly
        Assert.DoesNotContain(services, s =>
            s.ServiceType == typeof(ICommandHandler<DummyCommandA>));
    }

    [Fact]
    public void AddCommandHandlersFromAssembly_RegistersAsScoped()
    {
        
        IServiceCollection services = new ServiceCollection();
        var applicationAssembly = typeof(CreatePlayerHandler).Assembly;

        
        services.AddCommandHandlersFromAssembly(applicationAssembly);

        // Assert all registrations should be Scoped
        var handlerRegistrations = services
            .Where(s => s.ServiceType.IsGenericType &&
                        s.ServiceType.GetGenericTypeDefinition() == typeof(ICommandHandler<>));

        Assert.All(handlerRegistrations, s =>
            Assert.Equal(ServiceLifetime.Scoped, s.Lifetime));
    }
}
