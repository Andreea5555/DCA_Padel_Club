using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;
using IntegrationTests.EfcQueriesTests.Helpers;
using Xunit;

namespace IntegrationTests.EfcQueriesTests.Handlers;

public class ScheduleOverviewHandlerTests
{
    [Fact]
    public async Task Returns_Only_Active_Schedules_For_April_2025()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ScheduleOverview.Query(2025, 4));

        Assert.Equal(2025, answer.Year);
        Assert.Equal(4, answer.Month);
        Assert.Equal(11, answer.Schedules.Count);
        Assert.All(answer.Schedules, s => Assert.Equal("Active", s.Status));
        Assert.All(answer.Schedules, s => Assert.StartsWith("2025-04", s.Date));
    }
}
