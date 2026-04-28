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
        CourtId? courtNumber = null
    )
    {
        var bookingId = new BookingId(Guid.NewGuid());
        var resolvedBooker = booker ?? new ViaId(1);
        var resolvedPlayers = players ?? new List<ViaId>();
        var resolvedCourt = courtNumber ?? CreateDefaultCourtId();

        return new BookingAggregate(
            bookingId,
            resolvedCourt,
            resolvedBooker,
            resolvedPlayers,
            CreateValidTimeSlot()
        );
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
        var field = typeof(BookingAggregate).GetField(
            "courtNumber",
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        Assert.NotNull(field);
        var value = field.GetValue(booking);
        Assert.NotNull(value);
        return Assert.IsType<CourtId>(value);
    }

    internal static List<ViaId> GetPlayerIds(BookingAggregate booking)
    {
        var field = typeof(BookingAggregate).GetField(
            "playerIds",
            BindingFlags.Instance | BindingFlags.NonPublic
        );
        Assert.NotNull(field);
        var value = field.GetValue(booking);
        Assert.NotNull(value);

        if (value is IList<ViaId> directPlayers)
        {
            return directPlayers.ToList();
        }

        var wrappedPlayers = Assert.IsAssignableFrom<System.Collections.IEnumerable>(value);
        return wrappedPlayers
            .Cast<object>()
            .Select(item =>
            {
                var playerIdProperty = item.GetType().GetProperty(
                    "PlayerId",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public
                );
                Assert.NotNull(playerIdProperty);
                return Assert.IsType<ViaId>(playerIdProperty.GetValue(item));
            })
            .ToList();
    }
}
