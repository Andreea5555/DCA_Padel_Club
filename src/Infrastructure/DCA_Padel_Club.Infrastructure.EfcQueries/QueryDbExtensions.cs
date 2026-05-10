using DCA_Padel_Club.Infrastructure.EfcQueries.SeedFactories;

namespace DCA_Padel_Club.Infrastructure.EfcQueries;

public static class QueryDbExtensions
{
    public static async Task SeedTestDataAsync(this ViapadelClubContext ctx)
    {
        var (players, guidToViaId) = PlayerSeeder.CreatePlayers();
        ctx.Players.AddRange(players);
        await ctx.SaveChangesAsync();

        var schedules = ScheduleSeeder.CreateSchedules();
        ctx.Schedules.AddRange(schedules);
        await ctx.SaveChangesAsync();

        var (courts, courtLookup) = ScheduleCourtSeeder.CreateScheduleCourts();
        ctx.ScheduleCourts.AddRange(courts);
        await ctx.SaveChangesAsync();

        var scheduleDates = schedules.ToDictionary(s => s.Id, s => s.Date);
        var bookings = BookingSeeder.CreateBookings(ctx, guidToViaId, courtLookup, scheduleDates);
        ctx.Bookings.AddRange(bookings);
        await ctx.SaveChangesAsync();
    }
}
