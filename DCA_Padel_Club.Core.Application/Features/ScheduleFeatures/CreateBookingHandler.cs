using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

namespace DCA_Padel_Club.Core.Application.Features.ScheduleFeatures;

public class CreateBookingHandler: ICommandHandler<CreateBookingCommand> //TODO ask if i shall add ICommandHandler here or not
{
    private readonly IScheduleRepository Repository;
    private readonly IUnitOfWork UnitOfWork;
    private readonly ICurrentDate CurrentDate;
    private readonly ICurrentTime CurrentTime;

    public CreateBookingHandler(
        IScheduleRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentDate currentDate,
        ICurrentTime currentTime)
    {
        this.Repository = repository;
        this.UnitOfWork = unitOfWork;
        this.CurrentDate = currentDate;
        this.CurrentTime = currentTime;
    }

    // TODO check with Troels what should Result have either None or Booking
    public async Task<Result<None>> HandleAsync(CreateBookingCommand command)
    {
        Schedule? schedule = await Repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<None>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }

        BookingSlot slot = new BookingSlot(
            command.Date,
            command.StartTime,
            command.EndTime);

        Result<None> result = schedule.CreateBooking(
            command.BookerId,
            command.CourtId,
            slot,
            CurrentDate,
            CurrentTime);

        if (result.IsSuccess)
        {
            await UnitOfWork.SaveChangesAsync();
        }

        return result;
    }
}