using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
public class CreateScheduleCommand
{
    private CreateScheduleCommand() { }

    public static Result<CreateScheduleCommand> Create()
    {
        return Result<CreateScheduleCommand>.Success(
            new CreateScheduleCommand());
    }
}