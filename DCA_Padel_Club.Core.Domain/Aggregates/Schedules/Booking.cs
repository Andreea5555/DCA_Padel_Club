using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Common.Bases;
using DCA_Padel_Club.Core.Tools.OperationResult;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public class Booking : Entity<BookingId>
{
    private CourtId courtNumber = null!;
    private ViaId bookerId = null!;
    private IList<BookingPlayerReference> playerIds = new List<BookingPlayerReference>();
    private BookingSlot bookingTimeSlot = null!;
    
    private Booking() : base(default!)
    {
    }

    internal Booking(
        BookingId id,
        CourtId courtNumber,
        ViaId bookerId,
        IList<ViaId> playerIds,
        BookingSlot timeSlot
    )
        : base(id)
    {
        this.courtNumber = courtNumber;
        this.bookerId = bookerId;

        if (playerIds is null)
        {
            throw new ArgumentNullException(nameof(playerIds));
        }

        this.playerIds = playerIds.Select(id => new BookingPlayerReference(id)).ToList();
        bookingTimeSlot = timeSlot;

        if (!HasPlayer(bookerId))
            this.playerIds.Add(new BookingPlayerReference(bookerId));
    }

    internal Result<None> AddPlayer(ViaId player)
    {
        if (HasPlayer(player))
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

        playerIds.Add(new BookingPlayerReference(player));
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

        if (!HasPlayer(player))
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

        BookingPlayerReference playerReference = playerIds.First(reference => reference.PlayerId == player);
        playerIds.Remove(playerReference);
        return Result<None>.Success(None.Value);
    }

    private bool HasPlayer(ViaId player)
    {
        return playerIds.Any(reference => reference.PlayerId == player);
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
