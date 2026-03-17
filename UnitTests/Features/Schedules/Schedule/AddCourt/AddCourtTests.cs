using DCA_Padel_Club.Core.Tools.OperationResult;
using UnitTests.Helpers;

namespace UnitTests.Features.Schedules.Schedule.AddCourt;
using  DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
public class AddCourtTests
{
    private Schedule CreateSchedule()
    {
        Schedule schedule= new Schedule();
        
        return schedule;
    }

    private Result<CourtId> CreateCourtId()
    {
        var courtId = CourtId.CreateCourtId("S2");
        return courtId;
    }

    [Fact]
    public void AddCourt_Date_InThe_Past()
    {
        var schedule = CreateSchedule();
        schedule.UpdateSchedule(DateOnly.FromDateTime(DateTime.Now).AddDays(-1), FakeCurrentDate.RealNow());
        
        // schedule.AddCourt(CreateCourtId(),false,false);
    }
}