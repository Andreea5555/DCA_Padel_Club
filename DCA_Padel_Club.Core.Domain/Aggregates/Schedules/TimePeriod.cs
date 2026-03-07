using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class TimePeriod : ValueObject
{
    private DateTime StartTime { get; set; }
    private DateTime EndTime { get; set; }

    private TimePeriod(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;  
    }
    
    private static List<OperationError> Validate(DateTime startTime, DateTime endTime)
    {
        var errors = new List<OperationError>();

        if (startTime.Kind != DateTimeKind.Utc)
            errors.Add(OperationError.Create("TimePeriod.Start.NotUtc", "Start time must be in UTC"));

        if (endTime.Kind != DateTimeKind.Utc)
            errors.Add(OperationError.Create("TimePeriod.End.NotUtc", "End time must be in UTC"));
        
        if (startTime >= endTime)
            errors.Add(OperationError.Create("TimePeriod.Range.Invalid", "Start time must be strictly earlier than end time"));

        return errors;
    }

    
    public static Result<TimePeriod> Create(DateTime startTime, DateTime endTime)
    {
        var errors = Validate(startTime, endTime);
        if (errors.Count > 0)
        {
            return Result<TimePeriod>.Failure(errors);
        }

        return Result<TimePeriod>.Success(new TimePeriod(startTime, endTime));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }
}
