using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class ActivateScheduleHandler: ICommandHandler<ActivateScheduleCommand>
{
    private readonly IUnitOfWork Uow;
    private readonly IScheduleRepository Repository;
    private readonly IActiveScheduleOnDate ActiveScheduleOnDate;
    private readonly ICurrentDate CurrentDate;
    private readonly ICurrentTime CurrentTime;

    public ActivateScheduleHandler(IScheduleRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentDate currentDate,
        ICurrentTime currentTime, IActiveScheduleOnDate activeScheduleOnDate)
    {
        Uow = unitOfWork;
        Repository = repository;
        CurrentDate = currentDate;
        CurrentTime = currentTime;
        ActiveScheduleOnDate = activeScheduleOnDate;
    }
    public async Task<Result<None>> HandleAsync(ActivateScheduleCommand command)
    {
        Schedule? schedule = await Repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }
        Result<None> result= schedule.ActivateSchedule(ActiveScheduleOnDate,CurrentDate, CurrentTime);
        if (result.IsSuccess)
        {
            await Uow.SaveChangesAsync();
        }
        
        return result;
    }
}