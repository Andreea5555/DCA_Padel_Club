using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedule;
using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Domain.Common.Contracts;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class Schedule : AggregateRoot<ScheduleId>
{
    internal DateOnly Date { get; private set; }
    internal TimeOnly StartTime { get; private set; }
    internal TimeOnly EndTime { get; private set; }
    internal Boolean IsDraft;
    internal IList<PadelCourt> Courts { get; private set; }
    internal bool IsDeleted;
    internal IList<Booking> Bookings;

    private Schedule(ScheduleId id) : base(id)
    {
        Date= DateOnly.FromDateTime(DateTime.Now);
        StartTime = TimeOnly.Parse("15:00:00");
        EndTime = TimeOnly.Parse("22:00:00");
        IsDraft = true;
        Courts = new List<PadelCourt>();
        IsDeleted = false;
        Bookings = new List<Booking>();
    }

    public static Schedule Create()
    {
        return new Schedule(new ScheduleId(Guid.NewGuid()));
    }

    //needed to check in the database and implement USe Case ID2, F1
    public Result<None> UpdateScheduledTimes( TimeOnly startTime, TimeOnly endTime)
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
            return Result<None>.Failure(errors);
        }
        
        return Result<None>.Success(new None());
    }

    //needed to check in the database and implement USe Case ID2, F1
    public Result<Schedule> UpdateScheduledDate(DateOnly date, ICurrentDate currentDate)
    {
        Date = date;
        var errors = new List<OperationError>();
        if (!IsDraft)
        {
            errors.Add(OperationError.Create("Schedule.IsNotDraft", "The schedule cannot be active while updating it."));
        }
        if (date < currentDate.Now)
        {
            errors.Add(OperationError.Create("Schedule.InvalidDate","The date chosen has already passed"));
        }
        if (errors.Count > 0)
        {
            return Result<Schedule>.Failure(errors);
        }
        return Result<Schedule>.Success(this);
    }
    
    //Not finished, implementation for UseCase ID3: F6 is needed since it's related to the database as well
    public Result<None> AddCourt(CourtId courtId, ICurrentDate currentDate)
    {
        var errors = new List<OperationError>();

        if (IsDeleted)
        {
            errors.Add(OperationError.Create("Schedule.Deleted", "The schedule has been deleted and courts cannot be added."));
        }

        if(Date< currentDate.Now)
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


        if (errors.Count > 0)
        {
            return Result<None>.Failure(errors);
        }
        Courts.Add(new PadelCourt(courtId));
        
        return Result<None>.Success(None.Value);
    }

    
    public Result<None> RemoveCourt(CourtId courtId)
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

    public Result<None> ActivateSchedule(IActiveScheduleOnDate activeScheduleOnDate, ICurrentDate currentDate, ICurrentTime currentTime)
    {
        var errors = new List<OperationError>();

        if (IsDeleted)
        {
            errors.Add(OperationError.Create("Schedule.Deleted", "The schedule has been deleted and cannot be activated."));
        }

        if (Courts.Count == 0)
        {
            errors.Add(OperationError.Create("Schedule.NoCourtsAvailable", "There are no courts in the schedule, please add at least one court before activation."));
        }

        if (Date < currentDate.Now && StartTime < currentTime.Now)
        {
            errors.Add(OperationError.Create("Schedule.InvalidStartTime","The start time chosen for the schedule has already passed"));
        }

        if (activeScheduleOnDate.ExistsActiveScheduleOn(Date))
        {
            errors.Add(OperationError.Create("Schedule.DateConflict", "Another active schedule already exists for this date."));
        }

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


    //still needs work
    public Result<None> DeleteSchedule(ICurrentDate currentDate)
    {
        var errors = new List<OperationError>();

        if (Date <= currentDate.Now)
        {
            errors.Add(OperationError.Create("Schedule.InvalidRemoval", "The removal of the schedule is not possible because the date has either passed or is happening already."));
        }

        if (IsDeleted) // need to check when we can if the schedule is not empty/ not created in the database
        {
            errors.Add(OperationError.Create("Schedule.Null", "No daily schedule has been found or the schedule has already been deleted"));
        }

        if (errors.Count > 0)
        {
            return Result<None>.Failure(errors);
        }

        if (IsDraft == false)
        {
            //all bookings deleted
            //players are notified
        }

        IsDeleted = true;
        Courts.Clear();
        return Result<None>.Success(None.Value);
    }
    
    public Result<None> CreateBooking(ViaId bookerId, CourtId courtId, BookingSlot slot, ICurrentDate currentDate, ICurrentTime currentTime)
    {
        var errors = new List<OperationError>();

        if (slot.Date < currentDate.Now || (slot.Date == currentDate.Now && slot.StartTime < currentTime.Now))
            errors.Add(OperationError.Create("Schedule.BookingInPast",
                "The booking slot starts before the current time."));

        if (IsDeleted)
            errors.Add(OperationError.Create("Schedule.Deleted", "The schedule has been deleted and cannot accept new bookings."));

        if (IsDraft)
            errors.Add(OperationError.Create("Schedule.IsDraft", "The schedule is still in draft and cannot accept new bookings."));

        if (!Courts.Any(c => c.GetID() == courtId.GetValue()))
            errors.Add(OperationError.Create("Schedule.CourtNotFound", "The requested court does not exist in this schedule."));

        errors.AddRange(slot.ValidateFitsWithin(StartTime, EndTime));

        if (Bookings.Any(b => b.IsOnCourtAndOverlaps(courtId, slot)))
            errors.Add(OperationError.Create("Schedule.BookingOverlap",
                "The requested time slot overlaps with an existing booking on this court."));

        if (Bookings.Any(b => b.IsBookedBy(bookerId)))
            errors.Add(OperationError.Create("Schedule.PlayerAlreadyHasBooking",
                "The player already has a booking on this date."));

        if (LeavesHoleOnCourt(courtId, slot))
            errors.Add(OperationError.Create("Schedule.BookingLeavesHole",
                "The booking would leave a gap shorter than 1 hour."));

        if (errors.Count > 0)
            return Result<Booking>.Failure(errors);

        var booking = new Booking(
            new BookingId(Guid.NewGuid()),
            courtId,
            bookerId,
            new List<ViaId>(),
            BookingStatus.Pending,
            slot);

        Bookings.Add(booking);
        return Result<None>.Success(new None());
    }

    private bool LeavesHoleOnCourt(CourtId courtId, BookingSlot slot)
    {
        TimeOnly prevEnd = StartTime;
        TimeOnly nextStart = EndTime;

        foreach (var b in Bookings.Where(b => b.IsOnCourt(courtId)))
        {
            var (start, end) = b.GetSlotBoundaries();
            if (end <= slot.StartTime && end > prevEnd)
                prevEnd = end;
            if (start >= slot.EndTime && start < nextStart)
                nextStart = start;
        }

        var gapBefore = slot.StartTime - prevEnd;
        var gapAfter = nextStart - slot.EndTime;

        return (gapBefore > TimeSpan.Zero && gapBefore < TimeSpan.FromHours(1)) ||
               (gapAfter > TimeSpan.Zero && gapAfter < TimeSpan.FromHours(1));
    }

}