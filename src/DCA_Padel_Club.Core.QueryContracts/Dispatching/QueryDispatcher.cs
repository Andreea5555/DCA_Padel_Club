using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Exceptions;

namespace DCA_Padel_Club.Core.QueryContracts.Dispatching;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        Type queryHandlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TAnswer));

        dynamic? handler = serviceProvider.GetService(queryHandlerType);

        if (handler is null)
        {
            throw new QueryHandlerNotFoundException(
                query.GetType().ToString(),
                typeof(TAnswer).ToString()
            );
        }

        return handler.HandleAsync((dynamic)query);
    }
}