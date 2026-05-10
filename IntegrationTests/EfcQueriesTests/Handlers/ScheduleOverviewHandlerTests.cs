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

    [Fact]
    public async Task Excludes_Deleted_April_7_Schedule_From_Player_View()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ScheduleOverview.Query(2025, 4));

        Assert.DoesNotContain(answer.Schedules, s => s.Date == "2025-04-07");
        Assert.DoesNotContain(answer.Schedules, s => s.Date == "2025-04-23");
    }

    [Fact]
    public async Task Excludes_All_Draft_Dates_From_Player_View()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ScheduleOverview.Query(2025, 4));

        var draftDates = new[] { "2025-04-18", "2025-04-21", "2025-04-24", "2025-04-26", "2025-04-27", "2025-04-29", "2025-04-30" };
        foreach (var date in draftDates)
        {
            Assert.DoesNotContain(answer.Schedules, s => s.Date == date);
        }
    }

    [Fact]
    public async Task Returns_Empty_For_Month_Without_Schedules()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ScheduleOverview.Query(2025, 5));

        Assert.Equal(2025, answer.Year);
        Assert.Equal(5, answer.Month);
        Assert.Empty(answer.Schedules);
    }

    [Fact]
    public async Task Returns_Schedules_Sorted_Ascending_By_Date()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new ScheduleOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new ScheduleOverview.Query(2025, 4));

        var dates = answer.Schedules.Select(s => s.Date).ToList();
        var sorted = dates.OrderBy(d => d, StringComparer.Ordinal).ToList();
        Assert.Equal(sorted, dates);
    }
}
