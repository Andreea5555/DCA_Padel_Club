using Microsoft.EntityFrameworkCore;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;

public class ScheduleOverviewHandler(ViapadelClubContext context)
    : IQueryHandler<ScheduleOverview.Query, ScheduleOverview.Answer>
{
    public async Task<ScheduleOverview.Answer> HandleAsync(ScheduleOverview.Query query)
    {
        string yearMonthPrefix = $"{query.Year:D4}-{query.Month:D2}";

        var schedules = await context.Schedules
            .Where(s => s.Date.StartsWith(yearMonthPrefix)
                        && s.IsDeleted == 0
                        && s.IsDraft == 0)
            .OrderBy(s => s.Date)
            .Select(s => new ScheduleOverview.ScheduleInfo(
                s.Id,
                s.Date,
                "Active"))
            .ToListAsync();

        return new ScheduleOverview.Answer(query.Year, query.Month, schedules);
    }
}
