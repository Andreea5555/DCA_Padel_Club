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

    [Fact]
    public async Task Splits_Alice_3_Bookings_Correctly_When_Now_Is_April_7_Midnight()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var fakeNow = new FakeCurrentTime(new DateTime(2025, 4, 7, 0, 0, 0));
        var handler = new PlayerPageHandler(ctx, fakeNow);

        var answer = await handler.HandleAsync(new PlayerPage.Query(123456));

        Assert.Equal(3, answer.UpcomingBookings.Count + answer.PastBookings.Count);
        Assert.Equal(1, answer.UpcomingBookings.Count);
        Assert.Equal(2, answer.PastBookings.Count);
        Assert.Equal(1, answer.UpcomingBookingsCount);

        Assert.Equal("2025-04-14", answer.UpcomingBookings[0].Date);
        Assert.Equal("14:00:00", answer.UpcomingBookings[0].StartTime);
        Assert.Equal("D5", answer.UpcomingBookings[0].CourtId);

        Assert.Equal("2025-04-06", answer.PastBookings[0].Date);
        Assert.Equal("19:00:00", answer.PastBookings[0].StartTime);
        Assert.Equal("S1", answer.PastBookings[0].CourtId);

        Assert.Equal("2025-04-01", answer.PastBookings[1].Date);
        Assert.Equal("10:30:00", answer.PastBookings[1].StartTime);
        Assert.Equal("S1", answer.PastBookings[1].CourtId);
    }

    [Fact]
    public async Task Returns_All_Alice_Bookings_As_Past_When_Now_Is_End_Of_2025()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var fakeNow = new FakeCurrentTime(new DateTime(2025, 12, 31, 23, 59, 0));
        var handler = new PlayerPageHandler(ctx, fakeNow);

        var answer = await handler.HandleAsync(new PlayerPage.Query(123456));

        Assert.Equal(0, answer.UpcomingBookings.Count);
        Assert.Equal(0, answer.UpcomingBookingsCount);
        Assert.Equal(3, answer.PastBookings.Count);

        Assert.Equal("2025-04-14", answer.PastBookings[0].Date);
        Assert.Equal("2025-04-06", answer.PastBookings[1].Date);
        Assert.Equal("2025-04-01", answer.PastBookings[2].Date);
    }

    [Fact]
    public async Task Returns_All_Alice_Bookings_As_Upcoming_When_Now_Is_Before_Seed_Data()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var fakeNow = new FakeCurrentTime(new DateTime(2025, 1, 1, 0, 0, 0));
        var handler = new PlayerPageHandler(ctx, fakeNow);

        var answer = await handler.HandleAsync(new PlayerPage.Query(123456));

        Assert.Equal(3, answer.UpcomingBookings.Count);
        Assert.Equal(3, answer.UpcomingBookingsCount);
        Assert.Equal(0, answer.PastBookings.Count);

        Assert.Equal("2025-04-01", answer.UpcomingBookings[0].Date);
        Assert.Equal("2025-04-06", answer.UpcomingBookings[1].Date);
        Assert.Equal("2025-04-14", answer.UpcomingBookings[2].Date);
    }
}
