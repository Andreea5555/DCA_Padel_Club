using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class CreateScheduleHandler: ICommandHandler<CreateScheduleCommand>
{
    private readonly IScheduleRepository Repository;
    private readonly IUnitOfWork Uow;
    
    public CreateScheduleHandler(
        IScheduleRepository repository,
        IUnitOfWork unitOfWork)
    { 
        Repository = repository;
        Uow = unitOfWork;
    }

    public async Task<Result<None>> HandleAsync(CreateScheduleCommand command)
    {
        Schedule schedule = Schedule.Create();

        await Repository.AddAsync(schedule);
        await Uow.SaveChangesAsync();

        return Result<None>.Success(None.Value);
    }
    
    
}