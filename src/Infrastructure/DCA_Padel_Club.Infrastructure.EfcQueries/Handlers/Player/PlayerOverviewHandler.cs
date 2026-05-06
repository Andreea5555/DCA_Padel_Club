using Microsoft.EntityFrameworkCore;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;

public class PlayerOverviewHandler : IQueryHandler<PlayerOverview.Query, PlayerOverview.Answer>
{
    private readonly EfcDbContext _context;

    public PlayerOverviewHandler(EfcDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerOverview.Answer> HandleAsync(PlayerOverview.Query query)
    {
        var players = await _context.Players
            .Select(p => new PlayerOverview.PlayerInfo(
                p.Id.Value,
                p.FirstName.Value,
                p.LastName.Value,
                p.Email.Value,
                p.Blacklisted
            ))
            .ToListAsync();

        return new PlayerOverview.Answer(players);
    }
}

