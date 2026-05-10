using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Player;
using IntegrationTests.EfcQueriesTests.Helpers;
using IntegrationTests.Fakes;
using Xunit;

namespace IntegrationTests.EfcQueriesTests.Handlers;

public class PlayerPageHandlerTests
{
    [Fact]
    public async Task Returns_Player_With_Bookings_Split_By_Faked_Now()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var fakeNow = new FakeCurrentTime(new DateTime(2025, 4, 15, 12, 0, 0));
        var handler = new PlayerPageHandler(ctx, fakeNow);

        var answer = await handler.HandleAsync(new PlayerPage.Query(123456));

        Assert.Equal(123456, answer.PlayerId);
        Assert.Equal("Alice", answer.FirstName);
        Assert.Equal("Smith", answer.LastName);
        Assert.Equal("123456@via.dk", answer.Email);

        int totalBookings = answer.UpcomingBookings.Count + answer.PastBookings.Count;
        Assert.True(totalBookings > 0);
        Assert.Equal(answer.UpcomingBookingsCount, answer.UpcomingBookings.Count);

        Assert.All(answer.PastBookings, b =>
            Assert.True(string.Compare($"{b.Date}T{b.StartTime}", "2025-04-15T12:00:00", StringComparison.Ordinal) < 0));
        Assert.All(answer.UpcomingBookings, b =>
            Assert.True(string.Compare($"{b.Date}T{b.StartTime}", "2025-04-15T12:00:00", StringComparison.Ordinal) >= 0));
    }
}
