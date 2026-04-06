using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class UpdateScheduleDateCommand
{
    public ScheduleId ScheduleId { get; }
    public DateOnly Date { get; }

    private UpdateScheduleDateCommand(ScheduleId scheduleId, DateOnly date)
    {
        ScheduleId = scheduleId;
        Date = date;
    }

    public static Result<UpdateScheduleDateCommand> Create(string scheduleId, string date)
    {
        return Result<UpdateScheduleDateCommand>.Success(
            new UpdateScheduleDateCommand(
                new ScheduleId(Guid.Parse(scheduleId)),
                DateOnly.Parse(date)
            ));
    }
}