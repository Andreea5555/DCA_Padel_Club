using Microsoft.EntityFrameworkCore;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;

public class PlayerOverviewHandler(ViapadelClubContext context)
    : IQueryHandler<PlayerOverview.Query, PlayerOverview.Answer>
{
    public async Task<PlayerOverview.Answer> HandleAsync(PlayerOverview.Query query)
    {
        var players = await context.Players
            .Select(p => new PlayerOverview.PlayerInfo(
                p.Id,
                p.FirstName,
                p.LastName,
                p.Email,
                p.Blacklisted == 1
            ))
            .ToListAsync();

        return new PlayerOverview.Answer(players);
    }
}
