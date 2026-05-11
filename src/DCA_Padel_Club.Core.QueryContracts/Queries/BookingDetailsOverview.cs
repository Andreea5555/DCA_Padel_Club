using DCA_Padel_Club.Core.QueryContracts.Abstractions;

namespace DCA_Padel_Club.Core.QueryContracts.Queries;

public class BookingDetailsOverview
{
    public record Query(string BookingId) : IQuery<Answer>;

    public record Answer(
        string BookingId,
        string ScheduleId,
        string CourtId,
        string Date,
        string StartTime,
        string EndTime,
        int BookerId
    );
}