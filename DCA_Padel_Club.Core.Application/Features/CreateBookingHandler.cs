using DCA_Padel_Club.Core.Application.Commands.ICommandHandler;
using DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

namespace DCA_Padel_Club.Core.Application.Features;

public class CreateBookingHandler //TODO ask if i shall add ICommandHandler here or not
{
    private readonly IScheduleRepository repository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ICurrentDate currentDate;
    private readonly ICurrentTime currentTime;

    public CreateBookingHandler(
        IScheduleRepository repository,
        IUnitOfWork unitOfWork,
        ICurrentDate currentDate,
        ICurrentTime currentTime)
    {
        this.repository = repository;
        this.unitOfWork = unitOfWork;
        this.currentDate = currentDate;
        this.currentTime = currentTime;
    }

    // TODO check with Troels what should Result have either None or Booking
    public async Task<Result<Booking>> HandleAsync(CreateBookingCommand command)
    {
        Schedule? schedule = await repository.GetAsync(command.ScheduleId);

        if (schedule is null)
        {
            return Result<Booking>.Failure(new List<OperationError>
            {
                OperationError.Create("Schedule.NotFound", "No schedule found")
            });
        }

        BookingSlot slot = new BookingSlot(
            command.Date,
            command.StartTime,
            command.EndTime);

        Result<Booking> result = schedule.CreateBooking(
            command.BookerId,
            command.CourtId,
            slot,
            currentDate,
            currentTime);

        if (result.IsSuccess)
        {
            await unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}