using DCA_Padel_Club.Core.QueryContracts.Abstractions;

namespace DCA_Padel_Club.Core.QueryContracts.Queries;

public class ScheduleOverview
{
    public record Query(int Year, int Month) : IQuery<Answer>;

    public record Answer(
        int Year,
        int Month,
        List<ScheduleInfo> Schedules
    );

    public record ScheduleInfo(
        string ScheduleId,
        string Date,
        string Status
    );
}