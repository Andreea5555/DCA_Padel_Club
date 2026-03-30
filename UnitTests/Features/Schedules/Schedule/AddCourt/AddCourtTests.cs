using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Helpers;

namespace UnitTests.Features.Schedules.Schedule.AddCourt;
using  DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
public class AddCourtTests
{
    private static Schedule CreateSchedule() => Schedule.Create();

    private Result<CourtId> CreateCourtId()
    {
        var courtId = CourtId.CreateCourtId("S2");
        return courtId;
    }

    [Fact]
    public void AddCourt_WhenScheduleHasFutureDate_ReturnsSuccess()
    {
        var schedule = CreateSchedule();

        var result = schedule.AddCourt(CreateCourtId().value, TestDefaults.Now);

        Assert.False(result.IsFailure);
        Assert.Single(schedule.Courts);
    }

    [Fact]
    public void AddCourt_WhenNoCourtsPresentYet_AddsFirstCourtSuccessfully()
    {
        var schedule = CreateSchedule();
        Assert.Empty(schedule.Courts);

        var result = schedule.AddCourt(CreateCourtId().value, TestDefaults.Now);

        Assert.False(result.IsFailure);
        Assert.Single(schedule.Courts);
    }

    [Fact]
    public void AddCourt_WhenOneCourtAlreadyPresent_AddsSecondCourtSuccessfully()
    {
        var schedule = CreateSchedule();
        schedule.AddCourt(CourtId.CreateCourtId("S1").value, TestDefaults.Now);

        var result = schedule.AddCourt(CourtId.CreateCourtId("S2").value, TestDefaults.Now);

        Assert.False(result.IsFailure);
        Assert.Equal(2, schedule.Courts.Count);
    }

    [Fact]
    public void AddCourt_WhenScheduleIsInPast_ReturnsFailure()
    {
        var schedule = Schedule.Create();
        schedule.UpdateScheduledDate(DateOnly.FromDateTime(DateTime.Now).AddDays(-1), TestDefaults.Now);

        var result = schedule.AddCourt(CreateCourtId().value, TestDefaults.Now);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidDate");
    }

    [Fact]
    public void AddCourt_WhenScheduleIsDeleted_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        schedule.UpdateScheduledDate(DateOnly.FromDateTime(DateTime.Now).AddDays(1), TestDefaults.Now);
        schedule.DeleteSchedule(TestDefaults.Now);

        var result = schedule.AddCourt(CreateCourtId().value, TestDefaults.Now);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.Deleted");
    }

    [Fact]
    public void AddCourt_WhenSameCourtAddedTwice_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        schedule.AddCourt(CreateCourtId().value, TestDefaults.Now);

        var result = schedule.AddCourt(CreateCourtId().value, TestDefaults.Now);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.CourtAlreadyExist");
    }
}
