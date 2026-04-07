using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class DeleteScheduleCommand
{
     public ScheduleId ScheduleId;

     private DeleteScheduleCommand(ScheduleId scheduleId)
     {
          ScheduleId = scheduleId;
     }

     public static Result<DeleteScheduleCommand> Create(string scheduleId)
     {
          return Result<DeleteScheduleCommand>.Success(
               new DeleteScheduleCommand(
                    new ScheduleId(Guid.Parse(scheduleId))));
     }
}