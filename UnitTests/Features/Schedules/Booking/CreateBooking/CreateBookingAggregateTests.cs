using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Features.Schedules.Booking.Helpers;
using BookingAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Booking;

namespace UnitTests.Features.Schedules.Booking.CreateBooking;

public class CreateBookingAggregateTests
{
    [Fact]
    public void Ctor_WithNullPlayerList_ThrowsArgumentNullException()
    {
        var id = new BookingId(Guid.NewGuid());
        var courtId = BookingTestHelper.CreateDefaultCourtId();
        var booker = new ViaId(1);
        var slot = BookingTestHelper.CreateValidTimeSlot();

        var ex = Assert.Throws<ArgumentNullException>(() => new BookingAggregate(id, courtId, booker, null!, BookingStatus.Pending, slot));

        Assert.Equal("playerIds", ex.ParamName);
    }

    [Fact]
    public void Ctor_StoresCourtId_Correctly()
    {
        var courtId = BookingTestHelper.CreateDefaultCourtId();
        var booking = BookingTestHelper.CreateBooking(courtNumber: courtId);

        var stored = BookingTestHelper.GetCourtId(booking);

        Assert.Equal(courtId.GetValue(), stored.GetValue());
    }

    [Fact]
    public void Ctor_AddsBookerIfMissingFromPlayerList()
    {
        var booker = new ViaId(1);
        var other = new ViaId(2);
        var booking = BookingTestHelper.CreateBooking(booker, [other], BookingStatus.Pending);

        var players = BookingTestHelper.GetPlayerIds(booking);

        Assert.Contains(booker, players);
        Assert.Equal(2, players.Count);
    }

    [Fact]
    public void Ctor_DoesNotDuplicateBookerIfAlreadyPresent()
    {
        var booker = new ViaId(1);
        var booking = BookingTestHelper.CreateBooking(booker, [booker], BookingStatus.Pending);

        var players = BookingTestHelper.GetPlayerIds(booking);

        Assert.Single(players);
    }

    [Fact]
    public void Ctor_MakesDefensiveCopyOfPlayerList()
    {
        var booker = new ViaId(1);
        var p2 = new ViaId(2);
        var p3 = new ViaId(3);
        var input = new List<ViaId> { p2 };

        var booking = BookingTestHelper.CreateBooking(booker, input, BookingStatus.Pending);
        input.Add(p3);
        input.Remove(p2);

        var players = BookingTestHelper.GetPlayerIds(booking);

        Assert.Contains(booker, players);
        Assert.Contains(p2, players);
        Assert.DoesNotContain(p3, players);
        Assert.Equal(2, players.Count);
    }
}
