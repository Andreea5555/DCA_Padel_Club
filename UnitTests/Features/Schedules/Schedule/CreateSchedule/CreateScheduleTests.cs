using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;

namespace UnitTests.Features.Schedules.Schedule.CreateSchedule;

public class CreateScheduleTests
{
    [Fact]
    public void CreateSchedule_HasNonEmptyId()
    {
        var schedule = new ScheduleAggregate();

        Assert.NotEqual(Guid.Empty, schedule.ScheduleId);
    }

    [Fact]
    public void CreateSchedule_IsDraft()
    {
        var schedule = new ScheduleAggregate();

        Assert.True(schedule.IsDraft);
    }

    [Fact]
    public void CreateSchedule_HasEmptyCourts()
    {
        var schedule = new ScheduleAggregate();

        Assert.Empty(schedule.Courts);
    }

    [Fact]
    public void CreateSchedule_DefaultStartTimeIs15()
    {
        var schedule = new ScheduleAggregate();

        Assert.Equal(TimeOnly.Parse("15:00"), schedule.StartTime);
    }

    [Fact]
    public void CreateSchedule_DefaultEndTimeIs22()
    {
        var schedule = new ScheduleAggregate();

        Assert.Equal(TimeOnly.Parse("22:00"), schedule.EndTime);
    }

    [Fact]
    public void CreateSchedule_DefaultDateIsToday()
    {
        var schedule = new ScheduleAggregate();

        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), schedule.Date);
    }

    [Fact]
    public void CreateSchedule_IsNotDeleted()
    {
        var schedule = new ScheduleAggregate();

        Assert.False(schedule.isDeleted);
    }
}
