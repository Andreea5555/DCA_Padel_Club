using System.Reflection;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using BookingAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Booking;
using BookingSlotType = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.BookingSlot;

namespace UnitTests.Features.Schedules.Booking.Helpers;

internal static class BookingTestHelper
{
    internal static BookingAggregate CreateBooking(
        ViaId? booker = null,
        IList<ViaId>? players = null,
        BookingStatus status = BookingStatus.Pending,
        CourtId? courtNumber = null)
    {
        var bookingId = new BookingId(Guid.NewGuid());
        var resolvedBooker = booker ?? new ViaId(1);
        var resolvedPlayers = players ?? new List<ViaId>();
        var resolvedCourt = courtNumber ?? CreateDefaultCourtId();

        return new BookingAggregate(bookingId, resolvedCourt, resolvedBooker, resolvedPlayers, status, CreateValidTimeSlot());
    }

    internal static CourtId CreateDefaultCourtId()
    {
        var result = CourtId.CreateCourtId("D1");
        Assert.False(result.IsFailure);
        return result.value;
    }

    internal static BookingSlotType CreateValidTimeSlot()
    {
        var date = new DateOnly(2026, 1, 1);
        var start = new TimeOnly(10, 0);
        var end = new TimeOnly(11, 0);
        var result = BookingSlotType.Create(date, start, end);
        Assert.False(result.IsFailure);
        return result.value;
    }

    internal static CourtId GetCourtId(BookingAggregate booking)
    {
        var field = typeof(BookingAggregate).GetField("courtNumber", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        var value = field.GetValue(booking);
        Assert.NotNull(value);
        return Assert.IsType<CourtId>(value);
    }

    internal static List<ViaId> GetPlayerIds(BookingAggregate booking)
    {
        var field = typeof(BookingAggregate).GetField("playerIds", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        var value = field.GetValue(booking);
        Assert.NotNull(value);
        var players = Assert.IsAssignableFrom<IList<ViaId>>(value);
        return players.ToList();
    }

    internal static BookingStatus GetStatus(BookingAggregate booking)
    {
        var field = typeof(BookingAggregate).GetField("bookingStatus", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        var value = field.GetValue(booking);
        Assert.NotNull(value);
        return Assert.IsType<BookingStatus>(value);
    }
}
