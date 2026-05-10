using Microsoft.EntityFrameworkCore;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;

public class PlayerPageHandler(ViapadelClubContext context, ICurrentTime currentTime)
    : IQueryHandler<PlayerPage.Query, PlayerPage.Answer>
{
    public async Task<PlayerPage.Answer> HandleAsync(PlayerPage.Query query)
    {
        var player = await context.Players
            .Where(p => p.Id == query.PlayerId)
            .Select(p => new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                p.Email,
                p.ProfilePicture
            })
            .SingleAsync();

        var bookingsRaw = await context.Bookings
            .Where(b => b.BookerId == query.PlayerId
                        || b.Players.Any(pp => pp.Id == query.PlayerId))
            .Select(b => new
            {
                b.BookingId,
                b.ScheduleId,
                b.SlotDate,
                b.SlotStartTime,
                b.SlotEndTime,
                b.CourtNumber
            })
            .ToListAsync();

        string nowIso = currentTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

        var allBookings = bookingsRaw
            .Select(b => new
            {
                Dto = new PlayerPage.BookingDto(
                    Guid.Parse(b.BookingId),
                    Guid.Parse(b.ScheduleId),
                    b.SlotDate,
                    b.SlotStartTime,
                    b.SlotEndTime,
                    b.CourtNumber),
                SortKey = $"{b.SlotDate}T{b.SlotStartTime}"
            })
            .ToList();

        var upcoming = allBookings
            .Where(b => string.Compare(b.SortKey, nowIso, StringComparison.Ordinal) >= 0)
            .OrderBy(b => b.SortKey)
            .Select(b => b.Dto)
            .ToList();

        var past = allBookings
            .Where(b => string.Compare(b.SortKey, nowIso, StringComparison.Ordinal) < 0)
            .OrderByDescending(b => b.SortKey)
            .Select(b => b.Dto)
            .ToList();

        return new PlayerPage.Answer(
            player.Id,
            player.FirstName,
            player.LastName,
            player.Email,
            player.ProfilePicture,
            upcoming.Count,
            upcoming,
            past
        );
    }
}
