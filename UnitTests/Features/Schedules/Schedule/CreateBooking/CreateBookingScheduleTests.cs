using System.Reflection;
using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Helpers;
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

        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

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

        var result = schedule.CreateBooking(bookerId, courtId, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

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

        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

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

        var result = schedule.CreateBooking(bookerId, courtId.value, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsDraft");
    }

    [Fact]
    public void CreateBooking_TwoBookingsOnDifferentCourts_BothStored()
    {
        var schedule = CreateActiveScheduleWithCourts("D1", "D2");
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 15, 0, 17, 0);

        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slotA, new FakeCurrentTime(new TimeOnly(0, 0)));
        schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D2").value, slotB, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.Equal(2, GetBookings(schedule).Count);
    }

    [Fact]
    public void CreateBooking_WhenCourtNotInSchedule_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var absentCourt = CourtId.CreateCourtId("D2").value;
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(new ViaId(1), absentCourt, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.CourtNotFound");
    }

    [Fact]
    public void CreateBooking_WhenSlotStartsBeforeScheduleStart_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1"); 
        var slot = CreateSlotUtc(schedule, 14, 0, 16, 0);  

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void CreateBooking_WhenSlotEndsAfterScheduleEnd_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var slot = CreateSlotUtc(schedule, 21, 0, 23, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void CreateBooking_WhenSlotDateDiffersFromScheduleDate_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var wrongDate = schedule.Date.AddDays(1);
        var slot = CreateSlotUtcOnDate(wrongDate, 15, 0, 17, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void CreateBooking_WhenOverlapsExistingBookingOnSameCourt_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var courtId = CourtId.CreateCourtId("D1").value;
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 16, 0, 18, 0); 

        schedule.CreateBooking(new ViaId(1), courtId, slotA, new FakeCurrentTime(new TimeOnly(0, 0)));
        var result = schedule.CreateBooking(new ViaId(2), courtId, slotB, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingOverlap");
    }

    [Fact]
    public void CreateBooking_WhenSameTimeSlotOnDifferentCourt_ReturnsSuccess()
    {
        var schedule = CreateActiveScheduleWithCourts("D1", "D2");
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 15, 0, 17, 0);

        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slotA, new FakeCurrentTime(new TimeOnly(0, 0)));
        var result = schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D2").value, slotB, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.False(result.IsFailure);
    }

    [Fact]
    public void CreateBooking_WhenSlotStartsBeforeCurrentTime_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var fakeTime = new FakeCurrentTime(new TimeOnly(18, 0));
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, fakeTime);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingInPast");
    }

    [Fact]
    public void CreateBooking_WhenPlayerAlreadyHasBookingOnSameDate_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourts("D1", "D2");
        var bookerId = new ViaId(1);
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 17, 0, 19, 0);

        schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slotA, new FakeCurrentTime(new TimeOnly(0, 0)));
        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D2").value, slotB, new FakeCurrentTime(new TimeOnly(0, 0)));

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.PlayerAlreadyHasBooking");
    }

    private static ScheduleAggregate CreateActiveScheduleWithCourt(string courtName)
    {
        var schedule = new ScheduleAggregate();
        var courtId = CourtId.CreateCourtId(courtName);
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false);
        schedule.ActivateSchedule(new FakeActiveScheduleOnDate(false));
        return schedule;
    }

    private static ScheduleAggregate CreateDeletedScheduleWithCourt(string courtName)
    {
        var schedule = new ScheduleAggregate();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1), FakeCurrentDate.RealNow());
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
        schedule.ActivateSchedule(new FakeActiveScheduleOnDate(false));
        return schedule;
    }
    
    private static BookingSlot CreateValidSlot(
        ScheduleAggregate schedule,
        int startHour, int startMinute,
        int endHour, int endMinute)
    {
        return CreateSlotOnDate(schedule.Date, startHour, startMinute, endHour, endMinute);
    }

    private static BookingSlot CreateSlotUtc(
        ScheduleAggregate schedule,
        int startHour, int startMinute,
        int endHour, int endMinute)
    {
        return CreateSlotOnDate(schedule.Date, startHour, startMinute, endHour, endMinute);
    }

    private static BookingSlot CreateSlotUtcOnDate(
        DateOnly date,
        int startHour, int startMinute,
        int endHour, int endMinute)
    {
        return CreateSlotOnDate(date, startHour, startMinute, endHour, endMinute);
    }

    private static BookingSlot CreateSlotOnDate(
        DateOnly date,
        int startHour, int startMinute,
        int endHour, int endMinute)
    {
        var start = new TimeOnly(startHour, startMinute);
        var end = new TimeOnly(endHour, endMinute);
        var result = BookingSlot.Create(date, start, end);
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
