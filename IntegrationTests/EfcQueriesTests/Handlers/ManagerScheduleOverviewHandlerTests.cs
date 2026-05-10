using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;
using IntegrationTests.EfcQueriesTests.Helpers;
using Xunit;

namespace IntegrationTests.EfcQueriesTests.Handlers;

public class ManagerScheduleOverviewHandlerTests
{
    [Fact]
    public async Task Returns_All_Schedules_With_Status_For_April_2025()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ManagerScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ManagerScheduleOverview.Query(2025, 4));

        Assert.Equal(2025, answer.Year);
        Assert.Equal(4, answer.Month);
        Assert.Equal(20, answer.Schedules.Count);
        Assert.Contains(answer.Schedules, s => s.Status == "Active");
        Assert.Contains(answer.Schedules, s => s.Status == "Draft");
        Assert.Contains(answer.Schedules, s => s.Status == "Deleted");
    }
}
