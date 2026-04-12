using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Application.Commands.ScheduleCommands;

public class CreateBookingCommand
{
    public ScheduleId ScheduleId { get; }
    public ViaId BookerId { get; }
    public CourtId CourtId { get; }
    public DateOnly Date { get; }
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }

    private CreateBookingCommand(
        ScheduleId scheduleId,
        ViaId bookerId,
        CourtId courtId,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime)
        => (ScheduleId, BookerId, CourtId, Date, StartTime, EndTime) =
            (scheduleId, bookerId, courtId, date, startTime, endTime);

    // TODO ask Troels if this is okay or if i'm having a stroke
    public static Result<CreateBookingCommand> Create(
        string scheduleId,
        int bookerId,
        string courtId,
        string date,
        string startTime,
        string endTime)
    {
        return Result<CreateBookingCommand>.Success(
            new CreateBookingCommand(
                new ScheduleId(Guid.Parse(scheduleId)),
                new ViaId(bookerId),
                new CourtId(courtId),
                DateOnly.Parse(date),
                TimeOnly.Parse(startTime),
                TimeOnly.Parse(endTime)));
    }
}