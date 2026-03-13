using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class Schedule
{
    internal Guid ScheduleId { get; private set; }
    internal DateOnly Date { get; private set; }
    internal TimeOnly StartTime { get; private set; }
    internal TimeOnly EndTime { get; private set; }
    internal Boolean IsDraft;
    internal IList<PadelCourt> Courts { get; private set; }
    internal IList<TimePeriod> AvailabilityPeriods;
    internal bool isDeleted;
    internal IList<Booking> bookings;

    public Schedule()
    {
        ScheduleId = Guid.NewGuid();
        Date= DateOnly.FromDateTime(DateTime.Now);
        StartTime = TimeOnly.Parse("15:00:00");
        EndTime = TimeOnly.Parse("20:00:00");
        IsDraft = true;
        Courts = new List<PadelCourt>();
        AvailabilityPeriods = new List<TimePeriod>();
        isDeleted = false;
        bookings = new List<Booking>();
    }

    //needed to check in the database and implement USe Case ID2, F1
    public Result<Schedule> UpdateSchedule( TimeOnly startTime, TimeOnly endTime)
    { 
        StartTime = startTime;
        EndTime = endTime;
        var errors = new List<OperationError>();
        
        if (startTime > endTime)
        {
            errors.Add(OperationError.Create("Schedule.StartTimeTooBig", "The start time cannot be greater than the end time of the schedule."));
        }

        if ((endTime - startTime).TotalMinutes < 60)
        {
            errors.Add(OperationError.Create("Schedule.InvalidDuration","The period between the start time and the end time is smaller than 60 minutes"));
        }
        if (!IsDraft)
        {
            errors.Add(OperationError.Create("Schedule.IsNotDraft", "The schedule cannot be active while updating it."));
        }

        if (startTime.Minute != 30 && startTime.Minute != 0||endTime.Minute != 30 && endTime.Minute != 0)
        {
            errors.Add(OperationError.Create("Schedule.InvalidMinute","The minutes of the start time and the end time must be either half or whole hours"));
        }
        if (errors.Count > 0)
        {
            return Result<Schedule>.Failure(errors);
        }
        
        return Result<Schedule>.Success(this);
    }

    //needed to check in the database and implement USe Case ID2, F1
    public Result<Schedule> UpdateSchedule( DateOnly date)
    {
        Date = date;
        var errors = new List<OperationError>();
        if (!IsDraft)
        {
            errors.Add(OperationError.Create("Schedule.IsNotDraft", "The schedule cannot be active while updating it."));
        }
        if(date< DateOnly.FromDateTime(DateTime.Now))
        {
            errors.Add(OperationError.Create("Schedule.InvalidDate","The date chosen has already passed"));
        }
        if (errors.Count > 0)
        {
            return Result<Schedule>.Failure(errors);
        }
        return Result<Schedule>.Success(this);
        
    }
    
    //Not finished, implementation for UseCase ID3: F3 is needed since it's related to the database as well
    public Result<None> AddCourt(CourtId courtId, bool isVIPEnabled, bool isOccupied)
    {
        var errors = new List<OperationError>();
        if(Date< DateOnly.FromDateTime(DateTime.Now))
        {
            errors.Add(OperationError.Create("Schedule.InvalidDate","The date chosen has already passed"));
        }

        if (Date == null)
        {
            errors.Add(OperationError.Create("Schedule.NullDate","No daily schedule has been found with this date"));
        }

        if (Courts.Any(c => c.GetID() == courtId.GetValue()))
        {
            errors.Add(OperationError.Create("Schedule.CourtAlreadyExist","The court already exists inside the schedule"));
        }

        if (Courts.Count == 0)
        {
            isOccupied = false;
        }

        if (errors.Count > 0)
        {
            return Result<None>.Failure(errors);
        }
        Courts.Add(new PadelCourt(courtId));
        
        return Result<None>.Success(None.Value);
    }

    
    public Result<None> RemoveCourt(CourtId courtId, bool isVIPEnabled, bool isOccupied)
    {
       var errors = new List<OperationError>();
       // TODO here the bookings are needed to complete
       if (Date == DateOnly.FromDateTime(DateTime.Now))
       {
           errors.Add(OperationError.Create("Schedule.DateIsNow","The court cannot be removed since the schedule is happening today"));
       }
       if (errors.Count > 0)
       {
            return Result<None>.Failure(errors);
       }
       Courts.Remove(new PadelCourt(courtId));
       
            return Result<None>.Success(None.Value);
    }
    

    public IList<PadelCourt> GetAvailableCourts()
    {
        IList<PadelCourt> courts= new List<PadelCourt>();
        foreach (PadelCourt court in Courts)
        {
            if (!court.isOccupied)
            {
                courts.Add(court);
            }
        }

        return courts;

    }

    // TODO create a domain contract to return a daily schedule by date
    public Result<None> ActivateSchedule( /*IScheduleByDate ...*/)
    {
        var errors = new List<OperationError>();   
        if (Courts.Count == 0)
        {
            errors.Add(OperationError.Create("Schedule.NoCourtsAvailable", "There are no courts in the schedule, please add at least one court before activation."));
        }

        if (Date < DateOnly.FromDateTime(DateTime.Now)&&StartTime< TimeOnly.FromDateTime(DateTime.Now))
        {
            errors.Add(OperationError.Create("Schedule.InvalidStartTime","The start time chosen for the schedule has already passed"));
        }

        //TODO failure scenario needed to see if the schedule exists -> related to the domain contract but also cannot finish until database is introduced in session 9

        if (IsDraft == false)
        {
            errors.Add(OperationError.Create("Schedule.IsActive","The chosen schedule is already active"));
        }

        if (errors.Count > 0)
        {
            return Result<None>.Failure(errors);
        }
        
        IsDraft = false;
        return Result<None>.Success(None.Value);
    }

    public void ArchiveSchedule()
    {
        IsDraft = true;
    }

    //still needs work
    public Result<None> RemoveSchedule()
    {
        var errors = new List<OperationError>();
        if (Date == DateOnly.FromDateTime(DateTime.Now)|| Date< DateOnly.FromDateTime(DateTime.Now))
        {
            errors.Add(OperationError.Create("Schedule.InvalidRemoval","The removal of the schedule is not possible because the date has either passed or is happening already."));
        }
        
        if (IsDraft == false)
        {
            //all bookings deleted
            //players are notified
        }

        if (isDeleted) //need to check when we can if the schedule is not empty/ not created in the database
        {
            errors.Add(OperationError.Create("Schedule.Null","No daily schedule has been found or the schedule has already been deleted"));
        }
            
        isDeleted = true;
        Courts.Clear();
        if (errors.Count > 0)
        {
            return Result<None>.Failure(errors);
        }
        
        return Result<None>.Success(None.Value);
    }
    
    public Result<Booking> CreateBooking(ViaId bookerId, CourtId courtId, TimePeriod slot)
    {
        var booking = new Booking(
            new BookingId(Guid.NewGuid()),
            courtId,
            bookerId,
            new List<ViaId>(),
            BookingStatus.Pending,
            slot);

        bookings.Add(booking);
        return Result<Booking>.Success(booking);
    }
    
}