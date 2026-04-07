using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class RemoveCourtCommand
{
    public ScheduleId ScheduleId { get; set; }
    public CourtId CourtId { get; set; }

    private RemoveCourtCommand(ScheduleId scheduleId, CourtId court)
    {   
        ScheduleId = scheduleId;
        CourtId = court;
    }
    
    public static Result<RemoveCourtCommand> Create(string scheduleId, string courtId)
    {
        return Result<RemoveCourtCommand>.Success(
            new RemoveCourtCommand(
                new ScheduleId(Guid.Parse(scheduleId)),
                new CourtId(courtId)));
    }
}