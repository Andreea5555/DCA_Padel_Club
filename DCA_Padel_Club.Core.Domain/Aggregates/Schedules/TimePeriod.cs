using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class TimePeriod : ValueObject
{
    private DateTime StartTime { get; }
    private DateTime EndTime { get; }

    private TimePeriod(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    private static List<OperationError> Validate(DateTime startTime, DateTime endTime)
    {
        var errors = new List<OperationError>();

        if (startTime.Minute != 0 && startTime.Minute != 30)
            errors.Add(OperationError.Create("TimePeriod.Start.InvalidMinutes",
                "Start time minutes must be :00 or :30"));

        if (endTime.Minute != 0 && endTime.Minute != 30)
            errors.Add(OperationError.Create("TimePeriod.End.InvalidMinutes",
                "End time minutes must be :00 or :30"));

        if (startTime >= endTime)
            errors.Add(OperationError.Create("TimePeriod.Range.Invalid",
                "Start time must be strictly earlier than end time"));

        return errors;
    }

    public static Result<TimePeriod> Create(DateTime startTime, DateTime endTime)
    {
        var errors = Validate(startTime, endTime);
        if (errors.Count > 0)
            return Result<TimePeriod>.Failure(errors);

        return Result<TimePeriod>.Success(new TimePeriod(startTime, endTime));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }
}
