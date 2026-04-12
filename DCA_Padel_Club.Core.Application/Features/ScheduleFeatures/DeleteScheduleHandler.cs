using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class DeleteScheduleHandler: ICommandHandler<DeleteScheduleCommand>
{
    private readonly IUnitOfWork Uow;
    private readonly IScheduleRepository Repository;
    private readonly ICurrentDate CurrentDate;

    public DeleteScheduleHandler(IUnitOfWork uow, IScheduleRepository scheduleRepository, ICurrentDate currentDate)
    {
        Uow = uow;
       Repository = scheduleRepository;
        CurrentDate = currentDate;
        
    }
    public async Task<Result<None>> HandleAsync(DeleteScheduleCommand command)
    {
        
        Schedule? schedule = await Repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }

        Result<None> result = schedule.DeleteSchedule(CurrentDate);
        if (result.IsSuccess)
        {
            await Uow.SaveChangesAsync();
        }
        
        return result;
    }
}