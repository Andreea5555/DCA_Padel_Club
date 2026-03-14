using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class BookingSlot : ValueObject
{
    internal DateOnly Date { get; }
    internal TimeOnly StartTime { get; }
    internal TimeOnly EndTime { get; }

    private BookingSlot(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        Date = date;
        StartTime = startTime;
        EndTime = endTime;
    }

    private static List<OperationError> Validate(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var errors = new List<OperationError>();

        if (startTime.Minute != 0 && startTime.Minute != 30)
            errors.Add(OperationError.Create("BookingSlot.Start.InvalidMinutes",
                "Start time minutes must be :00 or :30"));

        if (endTime.Minute != 0 && endTime.Minute != 30)
            errors.Add(OperationError.Create("BookingSlot.End.InvalidMinutes",
                "End time minutes must be :00 or :30"));

        if (startTime >= endTime)
        {
            errors.Add(OperationError.Create("BookingSlot.Range.Invalid",
                "Start time must be strictly earlier than end time"));
        }
        else
        {
            var duration = endTime - startTime;
            if (duration < TimeSpan.FromHours(1))
                errors.Add(OperationError.Create("BookingSlot.Duration.TooShort",
                    "Booking duration must be at least 1 hour"));

            if (duration > TimeSpan.FromHours(3))
                errors.Add(OperationError.Create("BookingSlot.Duration.TooLong",
                    "Booking duration must be at most 3 hours"));
        }

        return errors;
    }

    public static Result<BookingSlot> Create(DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var errors = Validate(date, startTime, endTime);
        if (errors.Count > 0)
            return Result<BookingSlot>.Failure(errors);

        return Result<BookingSlot>.Success(new BookingSlot(date, startTime, endTime));
    }

    internal List<OperationError> ValidateFitsWithin(DateOnly scheduleDate, TimeOnly scheduleStart, TimeOnly scheduleEnd)
    {
        var errors = new List<OperationError>();
        if (Date != scheduleDate || StartTime < scheduleStart || EndTime > scheduleEnd)
        {
            errors.Add(OperationError.Create("Schedule.SlotOutOfBounds",
                "The booking slot falls outside this schedule's date and time window."));
        }
        return errors;
    }

    internal bool Overlaps(BookingSlot other)
    {
        if (Date != other.Date)
            return false;
        return StartTime < other.EndTime && other.StartTime < EndTime;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Date;
        yield return StartTime;
        yield return EndTime;
    }
}
