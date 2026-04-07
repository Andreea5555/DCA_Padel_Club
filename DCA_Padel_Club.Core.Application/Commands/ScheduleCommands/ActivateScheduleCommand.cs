using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class ActivateScheduleCommand
{
    public ScheduleId ScheduleId { get; init; }

    private ActivateScheduleCommand(ScheduleId scheduleId)
    {
        ScheduleId = scheduleId;
    }

    public static Result<ActivateScheduleCommand> Create(string scheduleId)
    {
        return Result<ActivateScheduleCommand>.Success(
            new ActivateScheduleCommand(
                new ScheduleId(Guid.Parse(scheduleId))));
    }
}