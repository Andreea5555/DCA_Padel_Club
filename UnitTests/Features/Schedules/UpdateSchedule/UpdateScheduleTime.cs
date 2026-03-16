namespace UnitTests.Features.Schedules.UpdateSchedule;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
public class UpdateScheduleTime
{
    private Schedule CreateSchedule()
    {
        return new Schedule();
    }


    [Fact]
    public void UpdateScheduleTime_StartTime_Bigger_Than_EndTime()
    {
        var schedule = CreateSchedule();
        var startTime = TimeOnly.Parse("18:00");
        var endTime = TimeOnly.Parse("16:00");
        
        var result = schedule.UpdateSchedule(startTime, endTime);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages,
            e => e.ErrorCode == "Schedule.StartTimeTooBig");
    }

    [Fact]
    public void UpdateScheduleTime_Duration_IsLessThan_60_Minutes()
    {
        
        var schedule = CreateSchedule();
        var start = TimeOnly.Parse("14:00");
        var end = TimeOnly.Parse("14:30"); 
        
        var result = schedule.UpdateSchedule(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode== "Schedule.InvalidDuration");
    }
    
    [Fact]
    public void UpdateScheduleTime_Schedule_NotDraft()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false; 

        var start = TimeOnly.Parse("15:00");
        var end = TimeOnly.Parse("16:30");

        var result = schedule.UpdateSchedule(start, end);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
    }
    
    [Fact]
    public void UpdateSchedule_Minutes_Are_Invalid()
    {
        
        var schedule = CreateSchedule();
        var start = TimeOnly.Parse("14:10"); 
        var end = TimeOnly.Parse("15:45");   

        var result = schedule.UpdateSchedule(start, end);
        
        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidMinute");
    }
    
    [Fact]
    public void UpdateSchedule_Return_Multiple_Errors()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false; 
        var start = TimeOnly.Parse("15:45");
        var end = TimeOnly.Parse("15:30");
        
        var result = schedule.UpdateSchedule(start, end);
        
        Assert.True(result.IsFailure);
        Assert.Equal(3, result.errorMessages.Count);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.StartTimeTooBig");
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidMinute");
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
    }
    
    [Fact]
    public void UpdateSchedule_Success()
    {
        
        var schedule = CreateSchedule();
        schedule.IsDraft = true;
        var start = TimeOnly.Parse("14:00"); 
        var end = TimeOnly.Parse("15:30"); 
        
        var result = schedule.UpdateSchedule(start, end);
        
        Assert.False(result.IsFailure);
        Assert.Equal(start, schedule.StartTime);
        Assert.Equal(end, schedule.EndTime);
    }
}