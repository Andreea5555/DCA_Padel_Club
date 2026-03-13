using System.Reflection;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using BookingAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Booking;
using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;
    
namespace UnitTests.Features.Schedules.Schedule.CreateBooking;

public class CreateBookingScheduleTests
{
    [Fact]
    public void CreateBooking_WhenScheduleActiveAndCourtExists_ReturnsSuccessAndStoresBooking()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var bookerId = new ViaId(1);
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slot);

        Assert.False(result.IsFailure);
        Assert.NotNull(result.value);

        var bookings = GetBookings(schedule);
        Assert.Single(bookings);
    }

    [Fact]
    public void CreateBooking_BookingHasCorrectBookerAndCourt()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var bookerId = new ViaId(42);
        var courtId = CourtId.CreateCourtId("D1").value;
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(bookerId, courtId, slot);

        Assert.False(result.IsFailure);
        var booking = result.value;
        Assert.Equal(courtId.GetValue(), GetCourtId(booking).GetValue());
        Assert.Contains(bookerId, GetPlayerIds(booking));
    }

    [Fact]
    public void CreateBooking_WhenScheduleIsDeleted_ReturnsFailure()
    {
        var schedule = CreateDeletedScheduleWithCourt("D1");
        var bookerId = new ViaId(1);
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slot);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.Deleted");
    }

    [Fact]
    public void CreateBooking_WhenScheduleIsDraft_ReturnsFailure()
    {
        var schedule = new ScheduleAggregate();
        var courtId = CourtId.CreateCourtId("D1");
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false);

        var bookerId = new ViaId(1);
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(bookerId, courtId.value, slot);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsDraft");
    }

    [Fact]
    public void CreateBooking_TwoBookingsOnDifferentCourts_BothStored()
    {
        var schedule = CreateActiveScheduleWithCourts("D1", "D2");
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 15, 0, 17, 0);

        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slotA);
        schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D2").value, slotB);

        Assert.Equal(2, GetBookings(schedule).Count);
    }

    private static ScheduleAggregate CreateActiveScheduleWithCourt(string courtName)
    {
        var schedule = new ScheduleAggregate();
        var courtId = CourtId.CreateCourtId(courtName);
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false);
        schedule.ActivateSchedule();
        return schedule;
    }

    // Sets a future date so RemoveSchedule() succeeds, producing a legitimately deleted schedule.
    private static ScheduleAggregate CreateDeletedScheduleWithCourt(string courtName)
    {
        var schedule = new ScheduleAggregate();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1));
        var courtId = CourtId.CreateCourtId(courtName);
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false);
        var removeResult = schedule.RemoveSchedule();
        Assert.False(removeResult.IsFailure);
        return schedule;
    }

    private static ScheduleAggregate CreateActiveScheduleWithCourts(params string[] courtNames)
    {
        var schedule = new ScheduleAggregate();
        foreach (var name in courtNames)
        {
            var courtId = CourtId.CreateCourtId(name);
            Assert.False(courtId.IsFailure);
            schedule.AddCourt(courtId.value, false, false);
        }
        schedule.ActivateSchedule();
        return schedule;
    }
    
    private static TimePeriod CreateValidSlot(
        ScheduleAggregate schedule,
        int startHour, int startMinute,
        int endHour, int endMinute)
    {
        var date = schedule.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var start = new DateTime(date.Year, date.Month, date.Day, startHour, startMinute, 0, DateTimeKind.Utc);
        var end   = new DateTime(date.Year, date.Month, date.Day, endHour,   endMinute,   0, DateTimeKind.Utc);
        var result = TimePeriod.Create(start, end);
        Assert.False(result.IsFailure);
        return result.value;
    }

    private static IList<BookingAggregate> GetBookings(ScheduleAggregate schedule)
    {
        var field = typeof(ScheduleAggregate)
            .GetField("bookings", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        return (IList<BookingAggregate>)field!.GetValue(schedule)!;
    }

    private static CourtId GetCourtId(BookingAggregate booking)
    {
        var field = typeof(BookingAggregate).GetField("courtNumber", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        return (CourtId)field!.GetValue(booking)!;
    }

    private static IList<ViaId> GetPlayerIds(BookingAggregate booking)
    {
        var field = typeof(BookingAggregate).GetField("playerIds", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.NotNull(field);
        return (IList<ViaId>)field!.GetValue(booking)!;
    }
}
