using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Helpers;
using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;

namespace UnitTests.Features.Schedules.Schedule.RemoveSchedule;

public class RemoveScheduleTests
{
    [Fact]
    public void RemoveSchedule_WhenDateIsInFuture_ReturnsSuccess_AndMarksDeleted()
    {
        var schedule = ScheduleAggregate.Create();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1), TestDefaults.Now);

        var result = schedule.RemoveSchedule(TestDefaults.Now);

        Assert.False(result.IsFailure);
        Assert.True(schedule.isDeleted);
    }

    [Fact]
    public void RemoveSchedule_WhenDateIsToday_ReturnsFailure_AndDoesNotMutateSchedule()
    {
        var schedule = ScheduleAggregate.Create();
        var courtsBefore = schedule.Courts.Count;

        var result = schedule.RemoveSchedule(TestDefaults.Now);

        Assert.True(result.IsFailure);
        Assert.False(schedule.isDeleted);
        Assert.Equal(courtsBefore, schedule.Courts.Count);
    }

    [Fact]
    public void RemoveSchedule_WhenDateIsInPast_ReturnsFailure_AndDoesNotMutateSchedule()
    {
        var schedule = ScheduleAggregate.Create();
        var dateField = typeof(ScheduleAggregate)
            .GetProperty("Date", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        dateField!.SetValue(schedule, DateOnly.FromDateTime(DateTime.Now).AddDays(-1));

        var courtsBefore = schedule.Courts.Count;

        var result = schedule.RemoveSchedule(TestDefaults.Now);

        Assert.True(result.IsFailure);
        Assert.False(schedule.isDeleted);
        Assert.Equal(courtsBefore, schedule.Courts.Count);
    }

    [Fact]
    public void RemoveSchedule_WhenAlreadyDeleted_ReturnsFailure_AndDoesNotMutateSchedule()
    {
        var schedule = ScheduleAggregate.Create();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1), TestDefaults.Now);
        schedule.RemoveSchedule(TestDefaults.Now);
        Assert.True(schedule.isDeleted);

        var result = schedule.RemoveSchedule(TestDefaults.Now);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.Null");
    }
}
