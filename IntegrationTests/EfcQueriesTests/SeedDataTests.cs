using DCA_Padel_Club.Infrastructure.EfcQueries;
using IntegrationTests.EfcQueriesTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace IntegrationTests.EfcQueriesTests;

public class SeedDataTests
{
    [Fact]
    public async Task Can_Instantiate_New_Database_And_Seed_Test_Data()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();

        await ctx.SeedTestDataAsync();

        Assert.Equal(20, await ctx.Players.CountAsync());
        Assert.Equal(20, await ctx.Schedules.CountAsync());
        Assert.Equal(42, await ctx.ScheduleCourts.CountAsync());
        Assert.Equal(38, await ctx.Bookings.CountAsync());
    }
}
