using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class UpdateScheduleDateHandler:ICommandHandler<UpdateScheduleDateCommand>
{
    private readonly IScheduleRepository Repository;
    private readonly IUnitOfWork UnitOfWork;
    private readonly ICurrentDate CurrentDate;

    public UpdateScheduleDateHandler(IScheduleRepository repository,
        IUnitOfWork unitOfWork, ICurrentDate currentDate)
    {
       Repository = repository;
       UnitOfWork = unitOfWork;
       CurrentDate = currentDate;
    }

    public async Task<Result<None>> HandleAsync(UpdateScheduleDateCommand command)
    {
        Schedule? schedule = await Repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }
        Result<None> result= schedule.UpdateScheduledDate(command.Date, CurrentDate);
        
        if (result.IsSuccess)
        {
            await UnitOfWork.SaveChangesAsync();
        }
        
        return result;

    }
}