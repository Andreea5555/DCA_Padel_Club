using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class AddCourtCommand
{
    public ScheduleId ScheduleId { get; }
    public CourtId CourtId { get; }

    private AddCourtCommand(ScheduleId scheduleId, CourtId courtId)
    {
        ScheduleId = scheduleId;
        CourtId = courtId;
    }

    public static Result<AddCourtCommand> Create(string scheduleId, string courtId)
    {
        return Result<AddCourtCommand>.Success(
            new AddCourtCommand(
                new ScheduleId(Guid.Parse(scheduleId)),
                new CourtId(courtId)));
    }
}