using Microsoft.EntityFrameworkCore;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;

public class ManagerScheduleOverviewHandler(ViapadelClubContext context)
    : IQueryHandler<ManagerScheduleOverview.Query, ManagerScheduleOverview.Answer>
{
    public async Task<ManagerScheduleOverview.Answer> HandleAsync(ManagerScheduleOverview.Query query)
    {
        string yearMonthPrefix = $"{query.Year:D4}-{query.Month:D2}";

        var rows = await context.Schedules
            .Where(s => s.Date.StartsWith(yearMonthPrefix))
            .OrderBy(s => s.Date)
            .Select(s => new
            {
                s.Id,
                s.Date,
                s.StartTime,
                s.EndTime,
                s.IsDraft,
                s.IsDeleted
            })
            .ToListAsync();

        var schedules = rows
            .Select(r => new ManagerScheduleOverview.ScheduleInfo(
                r.Id,
                r.Date,
                r.StartTime,
                r.EndTime,
                DeriveStatus(r.IsDraft, r.IsDeleted)))
            .ToList();

        return new ManagerScheduleOverview.Answer(query.Year, query.Month, schedules);
    }

    private static string DeriveStatus(int isDraft, int isDeleted) =>
        isDeleted == 1 ? "Deleted" :
        isDraft == 1 ? "Draft" :
        "Active";
}
