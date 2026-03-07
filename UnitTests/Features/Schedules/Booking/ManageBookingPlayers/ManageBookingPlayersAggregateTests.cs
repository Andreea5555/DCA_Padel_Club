using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Features.Schedules.Booking.Helpers;

namespace UnitTests.Features.Schedules.Booking.ManageBookingPlayers;

public class ManageBookingPlayersAggregateTests
{
    [Fact]
    public void AddPlayer_WhenPlayerAlreadyExists_ReturnsFailureAndDoesNotDuplicate()
    {
        var booker = new ViaId(1);
        var p2 = new ViaId(2);
        var booking = BookingTestHelper.CreateBooking(booker, [p2], BookingStatus.Pending);

        var result = booking.AddPlayer(p2);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.PlayerAlreadyAdded");
        var players = BookingTestHelper.GetPlayerIds(booking);
        Assert.Equal(2, players.Count);
    }

    [Fact]
    public void AddPlayer_WhenPlayerIsNew_AddsPlayer()
    {
        var booker = new ViaId(1);
        var p2 = new ViaId(2);
        var booking = BookingTestHelper.CreateBooking(booker, [], BookingStatus.Pending);

        var result = booking.AddPlayer(p2);

        Assert.False(result.IsFailure);
        var players = BookingTestHelper.GetPlayerIds(booking);
        Assert.Contains(p2, players);
        Assert.Equal(2, players.Count);
    }

    [Fact]
    public void RemovePlayer_WhenRemovingBooker_ReturnsFailure()
    {
        var booker = new ViaId(1);
        var booking = BookingTestHelper.CreateBooking(booker, [], BookingStatus.Pending);

        var result = booking.RemovePlayer(booker);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.RemovePlayer.Booker");
    }

    [Fact]
    public void RemovePlayer_WhenPlayerExists_RemovesPlayer()
    {
        var booker = new ViaId(1);
        var p2 = new ViaId(2);
        var booking = BookingTestHelper.CreateBooking(booker, [p2], BookingStatus.Pending);

        var result = booking.RemovePlayer(p2);

        Assert.False(result.IsFailure);
        var players = BookingTestHelper.GetPlayerIds(booking);
        Assert.DoesNotContain(p2, players);
        Assert.Single(players);
    }

    [Fact]
    public void RemovePlayer_WhenPlayerDoesNotExist_ReturnsFailureAndDoesNotChangeList()
    {
        var booker = new ViaId(1);
        var p2 = new ViaId(2);
        var missing = new ViaId(99);
        var booking = BookingTestHelper.CreateBooking(booker, [p2], BookingStatus.Pending);

        var before = BookingTestHelper.GetPlayerIds(booking).Count;
        var result = booking.RemovePlayer(missing);
        var after = BookingTestHelper.GetPlayerIds(booking).Count;

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Booking.RemovePlayer.NotFound");
        Assert.Equal(before, after);
    }
}
