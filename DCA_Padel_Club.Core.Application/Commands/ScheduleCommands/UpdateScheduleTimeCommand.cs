using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class UpdateScheduleTimeCommand
{
    public ScheduleId ScheduleId { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    private UpdateScheduleTimeCommand(ScheduleId scheduleId, TimeOnly startTime, TimeOnly endTime)
    {
        ScheduleId = scheduleId;
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Result<UpdateScheduleTimeCommand> Create(string scheduleId, string startTime, string endTime)
    {
        return Result<UpdateScheduleTimeCommand>.Success(
            new UpdateScheduleTimeCommand(
                new ScheduleId(Guid.Parse(scheduleId)),
                TimeOnly.Parse(startTime),
                TimeOnly.Parse(endTime)
            ));
    }
}