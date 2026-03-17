using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Helpers;

namespace UnitTests.Features.Schedules.Schedule.AddCourt;
using  DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
public class AddCourtTests
{
    private Schedule CreateSchedule()
    {
        return new Schedule();
    }

    private Result<CourtId> CreateCourtId()
    {
        var courtId = CourtId.CreateCourtId("S2");
        return courtId;
    }

    // UC3 S1 — success: valid court added to draft schedule with future date
    [Fact]
    public void AddCourt_WhenScheduleHasFutureDate_ReturnsSuccess()
    {
        var schedule = CreateSchedule();

        var result = schedule.AddCourt(CreateCourtId().value, false, false);

        Assert.False(result.IsFailure);
        Assert.Single(schedule.Courts);
    }

    // UC3 S2 — first court added: courts collection goes from 0 to 1
    [Fact]
    public void AddCourt_WhenNoCourtsPresentYet_AddsFirstCourtSuccessfully()
    {
        var schedule = CreateSchedule();
        Assert.Empty(schedule.Courts);

        var result = schedule.AddCourt(CreateCourtId().value, false, false);

        Assert.False(result.IsFailure);
        Assert.Single(schedule.Courts);
    }

    // UC3 S3 — second court added alongside existing one
    [Fact]
    public void AddCourt_WhenOneCourtAlreadyPresent_AddsSecondCourtSuccessfully()
    {
        var schedule = CreateSchedule();
        schedule.AddCourt(CourtId.CreateCourtId("S1").value, false, false);

        var result = schedule.AddCourt(CourtId.CreateCourtId("S2").value, false, false);

        Assert.False(result.IsFailure);
        Assert.Equal(2, schedule.Courts.Count);
    }

    // UC3 F1 — schedule date is in the past
    [Fact]
    public void AddCourt_WhenScheduleIsInPast_ReturnsFailure()
    {
        var schedule = new Schedule();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(-1), FakeCurrentDate.RealNow());

        var result = schedule.AddCourt(CreateCourtId().value, false, false);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.InvalidDate");
    }

    // UC3 F3 — schedule has been deleted
    [Fact]
    public void AddCourt_WhenScheduleIsDeleted_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(1), FakeCurrentDate.RealNow());
        schedule.RemoveSchedule();

        var result = schedule.AddCourt(CreateCourtId().value, false, false);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.Deleted");
    }

    // UC3 F7 — same court added twice
    [Fact]
    public void AddCourt_WhenSameCourtAddedTwice_ReturnsFailure()
    {
        var schedule = CreateSchedule();
        schedule.AddCourt(CreateCourtId().value, false, false);

        var result = schedule.AddCourt(CreateCourtId().value, false, false);

        Assert.True(result.IsFailure);
        Assert.Contains(result.errorMessages, e => e.ErrorCode == "Schedule.CourtAlreadyExist");
    }
}
