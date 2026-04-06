using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using UnitTests.Helpers;
using ScheduleAggregate = DCA_Padel_Club.Core.Domain.Aggregates.Schedules.Schedule;

namespace UnitTests.Features.Schedules.Schedule.RemoveCourt;

public class RemoveCourtTests
{
    [Fact]
    public void RemoveCourt_WhenScheduleDateIsToday_ReturnsFailure()
    {
        var schedule = ScheduleAggregate.Create();
        var courtId = CourtId.CreateCourtId("D1");
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, TestDefaults.Now);

        var result = schedule.RemoveCourt(courtId.value);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.DateIsNow");
        Assert.Single(schedule.Courts);
    }

    [Fact]
    public void RemoveCourt_WhenScheduleDateIsNotToday_RemovesCourt()
    {
        var schedule = ScheduleAggregate.Create();
        var futureDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
        var setDateResult = schedule.UpdateScheduledDate(futureDate, TestDefaults.Now);
        Assert.False(setDateResult.IsFailure);

        var courtId = CourtId.CreateCourtId("D1");
        Assert.False(courtId.IsFailure);
        schedule.AddCourt(courtId.value, TestDefaults.Now);

        var result = schedule.RemoveCourt(courtId.value);

        Assert.False(result.IsFailure);
        Assert.Empty(schedule.Courts);
    }
}
