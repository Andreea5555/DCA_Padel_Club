using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class RemoveCourtHandler: ICommandHandler<RemoveCourtCommand>
{
    private readonly IScheduleRepository Repository;
    private readonly IUnitOfWork Uow;

    public RemoveCourtHandler(IScheduleRepository repository,
        IUnitOfWork unitOfWork)
    {
        Repository = repository;
        Uow = unitOfWork;
    }
    public async Task<Result<None>> HandleAsync(RemoveCourtCommand command)
    {
        Schedule? schedule = await Repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }
        
        Result<None> result = schedule.RemoveCourt(command.CourtId);
        if (result.IsSuccess)
        {
            await Uow.SaveChangesAsync();
        }
        
        return result;
    }
}