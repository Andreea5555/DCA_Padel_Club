using DCA_Padel_Club.Core.Domain.Aggregates.Players;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

// we need this wrapper entity so EFC can persist the cross-aggregate list of player IDs as FK rows to Players.
internal class BookingPlayerReference
{
    internal ViaId PlayerId { get; private set; } = null!;

    internal BookingPlayerReference(ViaId playerId)
    {
        PlayerId = playerId;
    }

    private BookingPlayerReference()
    {
    }
}
