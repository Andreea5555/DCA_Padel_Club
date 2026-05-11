using DCA_Padel_Club.Core.QueryContracts.Queries;
using DCA_Padel_Club.Infrastructure.EfcQueries;
using DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;
using IntegrationTests.EfcQueriesTests.Helpers;
using Xunit;

namespace IntegrationTests.EfcQueriesTests.Handlers;

public class BookingDetailsOverviewHandlerTests
{
    [Fact]
    public async Task Returns_Booking_Details_When_Booking_Exists()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new BookingDetailsOverviewHandler(ctx);

        const string bookingId = "b2c3d4e5-0001-0000-0000-000000000001";

        var answer = await handler.HandleAsync(
            new BookingDetailsOverview.Query(bookingId));

        Assert.Equal(bookingId, answer.BookingId);
        Assert.Equal("c1e2a3f4-b5d6-c7e8-f9a0-b1d2c3e4f5a6", answer.ScheduleId);
        Assert.Equal("S1", answer.CourtId);
        Assert.Equal("2025-04-01", answer.Date);
        Assert.Equal("10:30:00", answer.StartTime);
        Assert.Equal("12:00:00", answer.EndTime);
        Assert.True(answer.BookerId > 0);
    }

    [Fact]
    public async Task Returns_Correct_Time_And_Court_For_Known_Booking()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new BookingDetailsOverviewHandler(ctx);

        const string bookingId = "b2c3d4e5-0001-0000-0000-000000000001";

        var answer = await handler.HandleAsync(
            new BookingDetailsOverview.Query(bookingId));

        Assert.Equal("S1", answer.CourtId);
        Assert.Equal("2025-04-01", answer.Date);
        Assert.Equal("10:30:00", answer.StartTime);
        Assert.Equal("12:00:00", answer.EndTime);
    }

    [Fact]
    public async Task Throws_When_Booking_Not_Found()
    {
        await using var ctx = await TestDbFactory.CreateMigratedReadContextAsync();
        await ctx.SeedTestDataAsync();

        var handler = new BookingDetailsOverviewHandler(ctx);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.HandleAsync(
                new BookingDetailsOverview.Query("00000000-0000-0000-0000-000000000000")));
    }
}