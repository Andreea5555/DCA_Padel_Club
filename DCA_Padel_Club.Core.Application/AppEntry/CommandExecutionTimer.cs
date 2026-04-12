using System.Diagnostics;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.AppEntry;

public class CommandExecutionTimer(ICommandDispatcher next) : ICommandDispatcher
{
    public async Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        Result<None> result = await next.DispatchAsync(command);

        TimeSpan elapsedTime = stopwatch.Elapsed;
        Console.WriteLine($"[CommandExecutionTimer] {typeof(TCommand).Name} executed in {elapsedTime.TotalMilliseconds} ms");

        return result;
    }
}
