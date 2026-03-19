using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;

namespace UnitTests.Features.Schedules.Schedule.CreateSchedule;

public class CreateScheduleTests
{
    [Fact]
    public void CreateSchedule_HasNonEmptyId()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.NotEqual(Guid.Empty, schedule.Id.Id);
    }

    [Fact]
    public void CreateSchedule_IsDraft()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.True(schedule.IsDraft);
    }

    [Fact]
    public void CreateSchedule_HasEmptyCourts()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.Empty(schedule.Courts);
    }

    [Fact]
    public void CreateSchedule_DefaultStartTimeIs15()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.Equal(TimeOnly.Parse("15:00"), schedule.StartTime);
    }

    [Fact]
    public void CreateSchedule_DefaultEndTimeIs22()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.Equal(TimeOnly.Parse("22:00"), schedule.EndTime);
    }

    [Fact]
    public void CreateSchedule_DefaultDateIsToday()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.Equal(DateOnly.FromDateTime(DateTime.Now), schedule.Date);
    }

    [Fact]
    public void CreateSchedule_IsNotDeleted()
    {
        var schedule = ScheduleAggregate.Create();

        Assert.False(schedule.IsDeleted);
    }
}
