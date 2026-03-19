namespace UnitTests.Features.Schedules.ActivateSchedule;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
using UnitTests.Helpers;
public class ActivateScheduleTests
{

    private static Schedule CreateSchedule() => Schedule.Create();

    private PadelCourt CreatePadelCourt()
    {
        var result = CourtId.CreateCourtId("S");

        return new PadelCourt(result.value);
    }

    [Fact]
    public void ActivateSchedule_Schedule_Has_Zero_Courts()
    {
        var schedule = CreateSchedule();
        schedule.Courts.Clear();

        var result= schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages,
            e => e.ErrorCode == "Schedule.NoCourtsAvailable");
    }

    [Fact]
    public void ActivateSchedule_While_Schedule_DateTime_Has_Passed()
    {
        var schedule = CreateSchedule();
        schedule.UpdateSchedule(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), TestDefaults.Now);
        schedule.UpdateSchedule(new TimeOnly(10, 0), new TimeOnly(12, 0));

        var pastTime = new FakeCurrentTime(new TimeOnly(14, 0));
        var result = schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, pastTime);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidStartTime");
    }

    [Fact]
    public void ActivateSchedule_Schedule_Already_Active()
    {
        var schedule = CreateSchedule();
        schedule.Courts.Add(CreatePadelCourt());
        schedule.IsDraft = false;

        var result = schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsActive");
    }

    [Fact]
    public void ActivateSchedule_WhenValid()
    {
        var schedule = CreateSchedule();
        schedule.Courts.Add(CreatePadelCourt());

        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TestDefaults.Now);
        schedule.UpdateSchedule(TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(4)));

        var result = schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, TestDefaults.Midnight);

        Assert.False(result.IsFailure);
        Assert.False(schedule.IsDraft);
    }

    [Fact]
    public void ActivateSchedule_WhenScheduleIsDeleted_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TestDefaults.Now);
        schedule.RemoveSchedule(TestDefaults.Now);

        var result = schedule.ActivateSchedule(TestDefaults.NoConflict, TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.Deleted");
    }

    [Fact]
    public void ActivateSchedule_WhenAnotherActiveScheduleExistsOnSameDate_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        schedule.Courts.Add(CreatePadelCourt());
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TestDefaults.Now);

        var result = schedule.ActivateSchedule(new FakeActiveScheduleOnDate(true), TestDefaults.Now, TestDefaults.Midnight);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.DateConflict");
    }
}
