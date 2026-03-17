using UnitTests.Helpers;
using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;

namespace UnitTests.Features.Schedules.UpdateSchedule;

public class UpdateScheduleDate
{
    private static ScheduleAggregate CreateSchedule() => new ScheduleAggregate();

    [Fact]
    public void UpdateScheduleDate_Schedule_NotDraft()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false;
        var newDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        var result = schedule.UpdateSchedule(newDate, FakeCurrentDate.RealNow());

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
    }

    [Fact]
    public void UpdateScheduleDate_FutureDate_OnDraftSchedule_ReturnsSuccess()
    {
        var schedule = CreateSchedule();
        var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));

        var result = schedule.UpdateSchedule(futureDate, FakeCurrentDate.RealNow());

        Assert.False(result.IsFailure);
        Assert.Equal(futureDate, schedule.Date);
    }

    [Fact]
    public void UpdateScheduleDate_DateInPast_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        var fakeNow = new FakeCurrentDate(new DateOnly(2026, 3, 17));
        var pastDate = new DateOnly(2026, 3, 10);

        var result = schedule.UpdateSchedule(pastDate, fakeNow);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidDate");
    }

    [Fact]
    public void UpdateScheduleDate_NotDraftAndDateInPast_ReturnsBothErrors()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false;
        var fakeNow = new FakeCurrentDate(new DateOnly(2026, 3, 17));
        var pastDate = new DateOnly(2026, 3, 10);

        var result = schedule.UpdateSchedule(pastDate, fakeNow);

        Assert.True(result.IsFailure);
        Assert.Equal(2, result.errorMessages.Count);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidDate");
    }
}