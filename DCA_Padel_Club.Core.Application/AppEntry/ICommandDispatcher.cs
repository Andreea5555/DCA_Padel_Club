using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.AppEntry;

public interface ICommandDispatcher
{
    Task<Result<None>> DispatchAsync<TCommand>(TCommand command);
}
