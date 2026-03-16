namespace UnitTests.Features.Schedules.ActivateSchedule;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
public class ActivateScheduleTests
{
    private Schedule CreateSchedule()
    {
        Schedule schedule= new Schedule();
        
        return schedule;
    }
    
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
        
        var result= schedule.ActivateSchedule();
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages,
            e => e.ErrorCode == "Schedule.NoCourtsAvailable");
    }

    [Fact]
    public void ActivateSchedule_While_Schedule_DateTime_Has_Passed()
    {
        var schedule = CreateSchedule();
        schedule.UpdateSchedule(
            DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));
        schedule.UpdateSchedule(TimeOnly.FromDateTime(DateTime.Now),
            TimeOnly.FromDateTime(DateTime.Now));

        var result = schedule.ActivateSchedule();
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidStartTime");
    }

    [Fact]
    public void ActivateSchedule_Schedule_Already_Active()
    {
        var schedule = CreateSchedule();
        schedule.Courts.Add(CreatePadelCourt());
        schedule.IsDraft = false;
        
        var result = schedule.ActivateSchedule();
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsActive");
    }

    [Fact]
    public void ActivateSchedule_WhenValid()
    {
        var schedule = CreateSchedule();
        schedule.Courts.Add(CreatePadelCourt());

        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        schedule.UpdateSchedule(TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
            TimeOnly.FromDateTime(DateTime.Now.AddHours(4)));
        
        var result = schedule.ActivateSchedule();
        
        Assert.False(result.IsFailure);
        Assert.False(schedule.IsDraft);
    }
}