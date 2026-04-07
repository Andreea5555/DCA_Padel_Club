using UnitTests.Fakes;
using UnitTests.Helpers;
using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;

namespace UnitTests.Features.Schedules.UpdateSchedule;

public class UpdateScheduleDate
{
    private static ScheduleAggregate CreateSchedule() => ScheduleAggregate.Create();

    [Fact]
    public void UpdateScheduleDate_Schedule_NotDraft()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false;
        var originalDate = schedule.Date;
        var newDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        var result = schedule.UpdateScheduledDate(newDate, FakeCurrentDate.RealNow());

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
        Assert.Equal(originalDate, schedule.Date);
    }

    [Fact]
    public void UpdateScheduleDate_FutureDate_OnDraftSchedule_ReturnsSuccess()
    {
        var schedule = CreateSchedule();
        var futureDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3));

        var result = schedule.UpdateScheduledDate(futureDate, FakeCurrentDate.RealNow());

        Assert.False(result.IsFailure);
        Assert.Equal(futureDate, schedule.Date);
    }

    [Fact]
    public void UpdateScheduleDate_DateInPast_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        var originalDate = schedule.Date;
        var fakeNow = new FakeCurrentDate(new DateOnly(2026, 3, 17));
        var pastDate = new DateOnly(2026, 3, 10);

        var result = schedule.UpdateScheduledDate(pastDate, fakeNow);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidDate");
        Assert.Equal(originalDate, schedule.Date);
    }

    [Fact]
    public void UpdateScheduleDate_NotDraftAndDateInPast_ReturnsBothErrors()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false;
        var fakeNow = new FakeCurrentDate(new DateOnly(2026, 3, 17));
        var pastDate = new DateOnly(2026, 3, 10);

        var result = schedule.UpdateScheduledDate(pastDate, fakeNow);

        Assert.True(result.IsFailure);
        Assert.Equal(2, result.errorMessages.Count);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidDate");
    }
}
