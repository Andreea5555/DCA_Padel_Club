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

    [Fact]
    public async Task Counts_Schedules_By_Status_For_April_2025()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ManagerScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ManagerScheduleOverview.Query(2025, 4));

        Assert.Equal(11, answer.Schedules.Count(s => s.Status == "Active"));
        Assert.Equal(7, answer.Schedules.Count(s => s.Status == "Draft"));
        Assert.Equal(2, answer.Schedules.Count(s => s.Status == "Deleted"));
    }

    [Fact]
    public async Task Specific_Schedules_Have_Expected_Status_Mapping()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ManagerScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ManagerScheduleOverview.Query(2025, 4));

        var deletedApril7 = answer.Schedules.Single(s => s.Date == "2025-04-07");
        Assert.Equal("Deleted", deletedApril7.Status);

        var draftApril18 = answer.Schedules.Single(s => s.Date == "2025-04-18");
        Assert.Equal("Draft", draftApril18.Status);

        var activeApril1 = answer.Schedules.Single(s => s.Date == "2025-04-01");
        Assert.Equal("Active", activeApril1.Status);
        Assert.Equal("10:30:00", activeApril1.StartTime);
        Assert.Equal("15:30:00", activeApril1.EndTime);
    }

    [Fact]
    public async Task Returns_Empty_For_Month_Without_Schedules()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ManagerScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ManagerScheduleOverview.Query(2025, 12));

        Assert.Empty(answer.Schedules);
    }
}
