using Microsoft.Extensions.DependencyInjection;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;

namespace DCA_Padel_Club.Infrastructure.EfcQueries;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfcQueryHandlers(this IServiceCollection services)
    {
  
        services.AddTransient<IQueryHandler<PlayerOverview.Query, PlayerOverview.Answer>, PlayerOverviewHandler>();
        services.AddTransient<IQueryHandler<PlayerPage.Query, PlayerPage.Answer>, PlayerPageHandler>();
        return services;
    }
}
