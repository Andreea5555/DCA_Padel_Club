using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;
using IntegrationTests.EfcQueriesTests.Helpers;
using Xunit;

namespace IntegrationTests.EfcQueriesTests.Handlers;

public class PlayerOverviewHandlerTests
{
    [Fact]
    public async Task Returns_All_20_Players_Including_Blacklist_Flag()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new PlayerOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new PlayerOverview.Query());

        Assert.Equal(20, answer.Players.Count);
        Assert.Equal(2, answer.Players.Count(p => p.Blacklisted));
        Assert.Contains(answer.Players, p => p.FirstName == "Ethan" && p.Blacklisted);
        Assert.Contains(answer.Players, p => p.FirstName == "Oscar" && p.Blacklisted);
    }

    [Fact]
    public async Task Returns_Alice_With_Correct_ViaId_And_Email()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new PlayerOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new PlayerOverview.Query());

        var alice = answer.Players.Single(p => p.FirstName == "Alice");
        Assert.Equal(123456, alice.PlayerId);
        Assert.Equal("Smith", alice.LastName);
        Assert.Equal("123456@via.dk", alice.Email);
        Assert.False(alice.Blacklisted);
    }

    [Fact]
    public async Task ViaId_Matches_Email_Prefix_For_Every_Player()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new PlayerOverviewHandler(ctx);

        var answer = await handler.HandleAsync(new PlayerOverview.Query());

        Assert.All(answer.Players, p =>
        {
            var prefix = p.Email[..p.Email.IndexOf('@')];
            Assert.Equal(p.PlayerId, int.Parse(prefix));
        });
    }
}
