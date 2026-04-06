using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class Booking : Entity<BookingId>
{
    private readonly CourtId courtNumber;
    private ViaId bookerId;
    private IList<ViaId> playerIds;
    private readonly BookingSlot bookingTimeSlot;

    public Booking(
        BookingId id,
        CourtId courtNumber,
        ViaId bookerId,
        IList<ViaId> playerIds,
        BookingSlot timeSlot
    )
        : base(id)
    {
        this.bookerId = bookerId;
        this.courtNumber = courtNumber;
        if (playerIds is null)
        {
            throw new ArgumentNullException(nameof(playerIds));
        }
        this.playerIds = new List<ViaId>(playerIds);
        bookingTimeSlot = timeSlot;

        if (!this.playerIds.Contains(bookerId))
            this.playerIds.Add(bookerId);
    }

    internal Result<None> AddPlayer(ViaId player)
    {
        if (playerIds.Contains(player))
        {
            return Result<None>.Failure(
                [
                    OperationError.Create(
                        "Booking.PlayerAlreadyAdded",
                        "Player is already part of this booking."
                    ),
                ]
            );
        }

        playerIds.Add(player);
        return Result<None>.Success(None.Value);
    }

    internal Result<None> RemovePlayer(ViaId player)
    {
        if (player == bookerId)
        {
            return Result<None>.Failure(
                [
                    OperationError.Create(
                        "Booking.RemovePlayer.Booker",
                        "Booker cannot be removed from booking."
                    ),
                ]
            );
        }

        if (!playerIds.Contains(player))
        {
            return Result<None>.Failure(
                [
                    OperationError.Create(
                        "Booking.RemovePlayer.NotFound",
                        "Player is not part of this booking."
                    ),
                ]
            );
        }

        playerIds.Remove(player);
        return Result<None>.Success(None.Value);
    }

    internal bool IsOnCourt(CourtId court)
    {
        return courtNumber.GetValue() == court.GetValue();
    }

    internal bool IsOnCourtAndOverlaps(CourtId court, BookingSlot slot)
    {
        return IsOnCourt(court) && bookingTimeSlot.Overlaps(slot);
    }

    internal (TimeOnly Start, TimeOnly End) GetSlotBoundaries()
    {
        return (bookingTimeSlot.StartTime, bookingTimeSlot.EndTime);
    }

    internal bool IsBookedBy(ViaId id)
    {
        return bookerId == id;
    }
}
