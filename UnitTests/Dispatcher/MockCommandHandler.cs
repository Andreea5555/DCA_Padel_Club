using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace UnitTests.Dispatcher;

public class MockCommandHandler<TCommand> : ICommandHandler<TCommand>
{
    public bool HandleAsyncWasCalled { get; private set; }
    public int HandleAsyncCallCount { get; private set; }
    public TCommand? ReceivedCommand { get; private set; }

    public Task<Result<None>> HandleAsync(TCommand command)
    {
        HandleAsyncWasCalled = true;
        HandleAsyncCallCount++;
        ReceivedCommand = command;
        return Task.FromResult(Result<None>.Success(None.Value));
    }
}
