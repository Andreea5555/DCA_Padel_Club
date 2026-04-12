using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Tools.OperationResult;
using Microsoft.Extensions.DependencyInjection;

namespace DCA_Padel_Club.Core.Application.AppEntry;

public class Dispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<Result<None>> DispatchAsync<TCommand>(TCommand command)
    {
        Type serviceType = typeof(ICommandHandler<TCommand>);
        var service = serviceProvider.GetService(serviceType);

        if (service == null)
        {
            throw new ServiceNotFoundException(nameof(ICommandHandler<TCommand>));
        }

        ICommandHandler<TCommand> handler = (ICommandHandler<TCommand>)service;
        return handler.HandleAsync(command);
    }
}
