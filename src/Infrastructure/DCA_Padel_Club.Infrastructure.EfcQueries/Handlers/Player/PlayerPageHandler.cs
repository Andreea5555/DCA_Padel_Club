using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;

public class PlayerPageHandler : IQueryHandler<PlayerPage.Query, PlayerPage.Answer>
{
    private readonly EfcDbContext _context;

    public PlayerPageHandler(EfcDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerPage.Answer> HandleAsync(PlayerPage.Query query)
    {
        var playerId = new ViaId(query.PlayerId);

        var player = await _context.Players
            .SingleOrDefaultAsync(p => p.Id == playerId);

        if (player is null)
            throw new InvalidOperationException($"Player with id {query.PlayerId} was not found.");

        string profilePicture = player.ProfilePicture?.Value ?? string.Empty;

        var bookingDtos = new List<PlayerPage.BookingDto>();

        var sql = @"SELECT b.BookingId, b.ScheduleId, b.SlotDate, b.SlotStartTime, b.SlotEndTime, b.CourtNumber
FROM Bookings b
LEFT JOIN BookingPlayers bp ON b.BookingId = bp.BookingId
WHERE b.BookerId = @playerId OR bp.PlayerId = @playerId";

        var connection = _context.Database.GetDbConnection();
        try
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            var param = cmd.CreateParameter();
            param.ParameterName = "@playerId";
            param.Value = playerId.Value;
            cmd.Parameters.Add(param);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var bookingId = reader.GetGuid(0);
                var scheduleId = reader.IsDBNull(1) ? Guid.Empty : reader.GetGuid(1);

                string dateStr = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
                string startTimeStr = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
                string endTimeStr = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                string courtId = reader.IsDBNull(5) ? string.Empty : reader.GetString(5);

                string date = dateStr;
                string startTime = startTimeStr;
                string endTime = endTimeStr;

                bookingDtos.Add(new PlayerPage.BookingDto(bookingId, scheduleId, date, startTime, endTime, courtId));
            }
        }
        finally
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                await connection.CloseAsync();
        }

        var now = DateTime.Now;
        var upcoming = new List<PlayerPage.BookingDto>();
        var past = new List<PlayerPage.BookingDto>();

        foreach (var b in bookingDtos)
        {
            DateTime slotStart;
            if (DateTime.TryParse($"{b.Date} {b.StartTime}", out slotStart))
            {
                if (slotStart >= now)
                    upcoming.Add(b);
                else
                    past.Add(b);
            }
            else
            {
                past.Add(b);
            }
        }

        upcoming = upcoming.OrderBy(b => DateTime.TryParse($"{b.Date} {b.StartTime}", out var d) ? d : DateTime.MaxValue).ToList();
        past = past.OrderByDescending(b => DateTime.TryParse($"{b.Date} {b.StartTime}", out var d) ? d : DateTime.MinValue).ToList();

        return new PlayerPage.Answer(
            query.PlayerId,
            player.FirstName.Value,
            player.LastName.Value,
            player.Email.Value,
            profilePicture,
            upcoming.Count,
            upcoming,
            past
        );
    }
}
