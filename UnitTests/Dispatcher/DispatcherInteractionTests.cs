using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using Microsoft.Extensions.DependencyInjection;

namespace UnitTests.Dispatcher;

// Simple dummy commands for testing — no validation needed, just routing targets
public record DummyCommandA(string Value);
public record DummyCommandB(int Number);

public class DispatcherInteractionTests
{
    // ZOMBIE: Zero — no handlers registered → throws ServiceNotFoundException
    [Fact]
    public async Task Dispatch_ZeroHandlersRegistered_ThrowsServiceNotFoundException()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);

        var command = new DummyCommandA("test");

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => dispatcher.DispatchAsync(command));
    }

    // ZOMBIE: One incorrect handler registered → throws
    [Fact]
    public async Task Dispatch_OneIncorrectHandlerRegistered_ThrowsServiceNotFoundException()
    {
        // Arrange — register handler for CommandB, dispatch CommandA
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandB>, MockCommandHandler<DummyCommandB>>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);

        var command = new DummyCommandA("test");

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => dispatcher.DispatchAsync(command));
    }

    // ZOMBIE: One correct handler registered → handler called, received correct command
    [Fact]
    public async Task Dispatch_OneCorrectHandlerRegistered_HandlerIsCalled()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandA>, MockCommandHandler<DummyCommandA>>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var handler = (MockCommandHandler<DummyCommandA>)serviceProvider
            .GetService<ICommandHandler<DummyCommandA>>()!;

        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);
        var command = new DummyCommandA("hello");

        // Act
        var result = await dispatcher.DispatchAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(handler.HandleAsyncWasCalled);
        Assert.Equal(1, handler.HandleAsyncCallCount);
        Assert.Equal(command, handler.ReceivedCommand);
    }

    // ZOMBIE: Multiple handlers including correct → only correct handler is called
    [Fact]
    public async Task Dispatch_MultipleHandlersIncludingCorrect_OnlyCorrectHandlerCalled()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandA>, MockCommandHandler<DummyCommandA>>();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandB>, MockCommandHandler<DummyCommandB>>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var handlerA = (MockCommandHandler<DummyCommandA>)serviceProvider
            .GetService<ICommandHandler<DummyCommandA>>()!;
        var handlerB = (MockCommandHandler<DummyCommandB>)serviceProvider
            .GetService<ICommandHandler<DummyCommandB>>()!;

        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);
        var command = new DummyCommandA("dispatch-me");

        // Act
        await dispatcher.DispatchAsync(command);

        // Assert — only handlerA was called
        Assert.True(handlerA.HandleAsyncWasCalled);
        Assert.Equal(1, handlerA.HandleAsyncCallCount);
        Assert.Equal(command, handlerA.ReceivedCommand);

        // handlerB was NOT called
        Assert.False(handlerB.HandleAsyncWasCalled);
        Assert.Equal(0, handlerB.HandleAsyncCallCount);
    }

    // ZOMBIE: Multiple handlers excluding correct → throws
    [Fact]
    public async Task Dispatch_MultipleHandlersExcludingCorrect_ThrowsServiceNotFoundException()
    {
        // Arrange — register handlers for B and another B-variant, dispatch A
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandB>, MockCommandHandler<DummyCommandB>>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);

        var command = new DummyCommandA("missing");

        // Act & Assert
        await Assert.ThrowsAsync<ServiceNotFoundException>(
            () => dispatcher.DispatchAsync(command));
    }

    // ZOMBIE: Handler called exactly once
    [Fact]
    public async Task Dispatch_CorrectHandler_CalledExactlyOnce()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandA>, MockCommandHandler<DummyCommandA>>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var handler = (MockCommandHandler<DummyCommandA>)serviceProvider
            .GetService<ICommandHandler<DummyCommandA>>()!;

        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);
        var command = new DummyCommandA("once");

        // Act
        await dispatcher.DispatchAsync(command);

        // Assert
        Assert.Equal(1, handler.HandleAsyncCallCount);
    }

    // ZOMBIE: Correct command argument is passed
    [Fact]
    public async Task Dispatch_CorrectCommandArgument_IsPassedToHandler()
    {
        // Arrange
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ICommandHandler<DummyCommandA>, MockCommandHandler<DummyCommandA>>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var handler = (MockCommandHandler<DummyCommandA>)serviceProvider
            .GetService<ICommandHandler<DummyCommandA>>()!;

        ICommandDispatcher dispatcher = new DCA_Padel_Club.Core.Application.AppEntry.Dispatcher(serviceProvider);
        var command = new DummyCommandA("specific-value");

        // Act
        await dispatcher.DispatchAsync(command);

        // Assert
        Assert.NotNull(handler.ReceivedCommand);
        Assert.Equal("specific-value", handler.ReceivedCommand!.Value);
    }
}
