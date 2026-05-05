using DCA_Padel_Club.Core.QueryContracts.Abstractions;

namespace DCA_Padel_Club.Core.QueryContracts.Queries;

public class DailyScheduleBookingOverview
{
    public record Query(string ScheduleId) : IQuery<Answer>;

    public record Answer(
        string Date,
        string StartTime,
        string EndTime,
        List<CourtInfo> Courts,
        List<BookingInfo> Bookings
    );

    public record CourtInfo(
        string CourtId
    );

    public record BookingInfo(
        string BookingId,
        string CourtId,
        string StartTime,
        string EndTime
    );
}