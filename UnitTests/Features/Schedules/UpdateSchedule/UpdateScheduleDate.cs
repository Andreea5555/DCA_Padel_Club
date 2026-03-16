namespace UnitTests.Features.Schedules.UpdateSchedule;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
public class UpdateScheduleDate
{
    private Schedule CreateSchedule()
    {
        return new Schedule();
    }
    
    [Fact]
    public void UpdateScheduleDate_Schedule_NotDraft()
    {
        var schedule = CreateSchedule();
        schedule.IsDraft = false; 
        var newDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        
        var result = schedule.UpdateSchedule(newDate);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.IsNotDraft");
    }
}