using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ICommandHandler;

public interface ICommandHandler<TCommand>
{
   Task<Result<None>> HandleAsync(TCommand command);
}