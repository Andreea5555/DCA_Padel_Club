using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;
using IntegrationTests.EfcQueriesTests.Helpers;
using Xunit;

namespace IntegrationTests.EfcQueriesTests.Handlers;

public class DailyScheduleBookingOverviewHandlerTests
{
    [Fact]
    public async Task Returns_Schedule_With_Courts_And_Bookings()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new DailyScheduleBookingOverviewHandler(ctx);

        var answer = await handler.HandleAsync(
            new DailyScheduleBookingOverview.Query("c1e2a3f4-b5d6-c7e8-f9a0-b1d2c3e4f5a6"));

        Assert.Equal("2025-04-01", answer.Date);
        Assert.Equal("10:30:00", answer.StartTime);
        Assert.Equal("15:30:00", answer.EndTime);
        Assert.Equal(2, answer.Courts.Count);
        Assert.Contains(answer.Courts, c => c.CourtId == "S1");
        Assert.Contains(answer.Courts, c => c.CourtId == "D1");
        Assert.Equal(4, answer.Bookings.Count);
    }
}
