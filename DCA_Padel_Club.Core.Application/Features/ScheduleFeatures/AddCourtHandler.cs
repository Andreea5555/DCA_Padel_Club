using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class AddCourtHandler: ICommandHandler<AddCourtCommand>
{
    private readonly IScheduleRepository Repository;
    private readonly IUnitOfWork Uow;
    private readonly ICurrentDate CurrentDate;

    public AddCourtHandler(IScheduleRepository repository,
        IUnitOfWork unitOfWork, ICurrentDate currentDate)
    {
        Repository = repository;
        Uow = unitOfWork;
        CurrentDate = currentDate;
    }

    public async Task<Result<None>> HandleAsync(AddCourtCommand command)
    {
        Schedule? schedule = await Repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }

        Result<None> result = schedule.AddCourt(command.CourtId, CurrentDate);
        if (result.IsSuccess)
        {
            await Uow.SaveChangesAsync();
        }
        
        return result;
    }
}