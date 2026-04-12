using DCA_Padel_Club.Core.Application.AppEntry;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace UnitTests.Dispatcher;

// A mock dispatcher that records whether DispatchAsync was called
public class MockCommandDispatcher : ICommandDispatcher
{
    public bool DispatchAsyncWasCalled { get; private set; }
    public int DispatchAsyncCallCount { get; private set; }

    private readonly Result<None> _resultToReturn;

    public MockCommandDispatcher(Result<None> resultToReturn)
    {
        _resultToReturn = resultToReturn;
    }

    public Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        DispatchAsyncWasCalled = true;
        DispatchAsyncCallCount++;
        return Task.FromResult(_resultToReturn);
    }
}

public class CommandExecutionTimerTests
{
    [Fact]
    public async Task DispatchAsync_DelegatesToInnerDispatcher()
    {
        // Arrange
        var innerDispatcher = new MockCommandDispatcher(Result<None>.Success(None.Value));
        var timerDecorator = new CommandExecutionTimer(innerDispatcher);
        var command = new DummyCommandA("test");

        // Act
        await timerDecorator.DispatchAsync(command);

        // Assert
        Assert.True(innerDispatcher.DispatchAsyncWasCalled);
        Assert.Equal(1, innerDispatcher.DispatchAsyncCallCount);
    }

    [Fact]
    public async Task DispatchAsync_ReturnsSuccessResult_WhenInnerReturnsSuccess()
    {
        // Arrange
        var innerDispatcher = new MockCommandDispatcher(Result<None>.Success(None.Value));
        var timerDecorator = new CommandExecutionTimer(innerDispatcher);
        var command = new DummyCommandA("test");

        // Act
        var result = await timerDecorator.DispatchAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DispatchAsync_ReturnsFailureResult_WhenInnerReturnsFailure()
    {
        // Arrange
        var errors = new List<OperationError>
        {
            new OperationError("TEST_ERROR", "Something went wrong")
        };
        var innerDispatcher = new MockCommandDispatcher(Result<None>.Failure(errors));
        var timerDecorator = new CommandExecutionTimer(innerDispatcher);
        var command = new DummyCommandA("test");

        // Act
        var result = await timerDecorator.DispatchAsync(command);

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task DispatchAsync_CompletesWithoutError()
    {
        // Arrange
        var innerDispatcher = new MockCommandDispatcher(Result<None>.Success(None.Value));
        var timerDecorator = new CommandExecutionTimer(innerDispatcher);
        var command = new DummyCommandA("test");

        // Act & Assert — no exception thrown
        var result = await timerDecorator.DispatchAsync(command);
        Assert.NotNull(result);
    }
}
