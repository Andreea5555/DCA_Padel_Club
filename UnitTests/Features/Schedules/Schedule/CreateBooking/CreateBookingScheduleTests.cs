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

        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slot, TestDefaults.Now, TestDefaults.Midnight);

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

        var result = schedule.CreateBooking(bookerId, courtId, slot, TestDefaults.Now, TestDefaults.Midnight);

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

        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slot, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.Deleted");
    }

    [Fact]
    public void CreateBooking_WhenScheduleIsDraft_ReturnsFailure()
    {
        var schedule = ScheduleAggregate.Create();
        var courtId = CourtId.CreateCourtId("D1");
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false, TestDefaults.Now);

        var bookerId = new ViaId(1);
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(bookerId, courtId.value, slot, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsDraft");
    }

    [Fact]
    public void CreateBooking_TwoBookingsOnDifferentCourts_BothStored()
    {
        var schedule = CreateActiveScheduleWithCourts("D1", "D2");
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 15, 0, 17, 0);

        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slotA, TestDefaults.Now, TestDefaults.Midnight);
        schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D2").value, slotB, TestDefaults.Now, TestDefaults.Midnight);

        Assert.Equal(2, GetBookings(schedule).Count);
    }

    [Fact]
    public void CreateBooking_WhenCourtNotInSchedule_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var absentCourt = CourtId.CreateCourtId("D2").value;
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(new ViaId(1), absentCourt, slot, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.CourtNotFound");
    }

    [Fact]
    public void CreateBooking_WhenSlotStartsBeforeScheduleStart_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var slot = CreateSlotUtc(schedule, 14, 0, 16, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void CreateBooking_WhenSlotEndsAfterScheduleEnd_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var slot = CreateSlotUtc(schedule, 21, 0, 23, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.SlotOutOfBounds");
    }

    [Fact]
    public void CreateBooking_WhenSlotDateDiffersFromScheduleDate_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var wrongDate = schedule.Date.AddDays(1);
        var slot = CreateSlotUtcOnDate(wrongDate, 15, 0, 17, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, TestDefaults.Now, TestDefaults.Midnight);

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

        schedule.CreateBooking(new ViaId(1), courtId, slotA, TestDefaults.Now, TestDefaults.Midnight);
        var result = schedule.CreateBooking(new ViaId(2), courtId, slotB, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingOverlap");
    }

    [Fact]
    public void CreateBooking_WhenSameTimeSlotOnDifferentCourt_ReturnsSuccess()
    {
        var schedule = CreateActiveScheduleWithCourts("D1", "D2");
        var slotA = CreateValidSlot(schedule, 15, 0, 17, 0);
        var slotB = CreateValidSlot(schedule, 15, 0, 17, 0);

        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slotA, TestDefaults.Now, TestDefaults.Midnight);
        var result = schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D2").value, slotB, TestDefaults.Now, TestDefaults.Midnight);

        Assert.False(result.IsFailure);
    }

    [Fact]
    public void CreateBooking_WhenSlotStartsBeforeCurrentTime_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        var fakeTime = new FakeCurrentTime(new TimeOnly(18, 0));
        var slot = CreateValidSlot(schedule, 15, 0, 17, 0);

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value, slot, TestDefaults.Now, fakeTime);

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

        schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D1").value, slotA, TestDefaults.Now, TestDefaults.Midnight);
        var result = schedule.CreateBooking(bookerId, CourtId.CreateCourtId("D2").value, slotB, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.PlayerAlreadyHasBooking");
    }

    [Fact]
    public void CreateBooking_WhenGapAfterExistingBookingIsTooShort_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 15, 0, 17, 0), TestDefaults.Now, TestDefaults.Midnight);

        var result = schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 17, 30, 19, 0), TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingLeavesHole");
    }

    [Fact]
    public void CreateBooking_WhenGapBeforeExistingBookingIsTooShort_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 18, 0, 20, 0), TestDefaults.Now, TestDefaults.Midnight);

        var result = schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 15, 0, 17, 30), TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingLeavesHole");
    }

    [Fact]
    public void CreateBooking_WhenGapAtScheduleStartIsTooShort_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 15, 30, 17, 0), TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingLeavesHole");
    }

    [Fact]
    public void CreateBooking_WhenGapAtScheduleEndIsTooShort_ReturnsFailure()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");

        var result = schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 19, 0, 21, 30), TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.BookingLeavesHole");
    }

    [Fact]
    public void CreateBooking_WhenBookingsAreAdjacentWithNoGap_ReturnsSuccess()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 15, 0, 17, 0), TestDefaults.Now, TestDefaults.Midnight);

        var result = schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 17, 0, 19, 0), TestDefaults.Now, TestDefaults.Midnight);

        Assert.False(result.IsFailure);
    }

    [Fact]
    public void CreateBooking_WhenGapIsExactlyOneHour_ReturnsSuccess()
    {
        var schedule = CreateActiveScheduleWithCourt("D1");
        schedule.CreateBooking(new ViaId(1), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 15, 0, 17, 0), TestDefaults.Now, TestDefaults.Midnight);

        var result = schedule.CreateBooking(new ViaId(2), CourtId.CreateCourtId("D1").value,
            CreateValidSlot(schedule, 18, 0, 20, 0), TestDefaults.Now, TestDefaults.Midnight);

        Assert.False(result.IsFailure);
    }

    private static ScheduleAggregate CreateActiveScheduleWithCourt(string courtName)
    {
        var schedule = ScheduleAggregate.Create();
        var courtId = CourtId.CreateCourtId(courtName);
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false, TestDefaults.Now);
        schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, TestDefaults.Midnight);
        return schedule;
    }

    private static ScheduleAggregate CreateDeletedScheduleWithCourt(string courtName)
    {
        var schedule = ScheduleAggregate.Create();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1), TestDefaults.Now);
        var courtId = CourtId.CreateCourtId(courtName);
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, false, false, TestDefaults.Now);
        var removeResult = schedule.RemoveSchedule(TestDefaults.Now);
        Assert.False(removeResult.IsFailure);
        return schedule;
    }

    private static ScheduleAggregate CreateActiveScheduleWithCourts(params string[] courtNames)
    {
        var schedule = ScheduleAggregate.Create();
        foreach (var name in courtNames)
        {
            var courtId = CourtId.CreateCourtId(name);
            Assert.False(courtId.IsFailure);
            schedule.AddCourt(courtId.value, false, false, TestDefaults.Now);
        }
        schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, TestDefaults.Midnight);
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
            .GetField("Bookings", BindingFlags.Instance | BindingFlags.NonPublic);
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
