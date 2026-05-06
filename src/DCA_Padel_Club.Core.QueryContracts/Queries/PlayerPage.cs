using DCA_Padel_Club.Core.QueryContracts.Abstractions;

namespace DCA_Padel_Club.Core.QueryContracts.Queries;

public class PlayerPage
{
    public record Query(int PlayerId) : IQuery<Answer>;

    public record Answer(
        int PlayerId,
        string FirstName,
        string LastName,
        string Email,
        string ProfilePicture,
        int UpcomingBookingsCount,
        List<BookingDto> UpcomingBookings,
        List<BookingDto> PastBookings
    );

    public record BookingDto(
        Guid BookingId,
        Guid ScheduleId,
        string Date,
        string StartTime,
        string EndTime,
        string CourtId
    );
}

