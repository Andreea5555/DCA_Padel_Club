using Microsoft.EntityFrameworkCore;
using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;

public class DailyScheduleBookingOverviewHandler(ViapadelClubContext context)
    : IQueryHandler<DailyScheduleBookingOverview.Query, DailyScheduleBookingOverview.Answer>
{
    public async Task<DailyScheduleBookingOverview.Answer> HandleAsync(DailyScheduleBookingOverview.Query query)
    {
        var result = await context.Schedules
            .Where(s => s.Id == query.ScheduleId)
            .Select(s => new
            {
                s.Date,
                s.StartTime,
                s.EndTime,
                Courts = s.ScheduleCourts
                    .Select(c => new DailyScheduleBookingOverview.CourtInfo(c.CourtNumber))
                    .ToList(),
                Bookings = s.Bookings
                    .Select(b => new DailyScheduleBookingOverview.BookingInfo(
                        b.BookingId,
                        b.CourtNumber,
                        b.SlotStartTime,
                        b.SlotEndTime))
                    .ToList()
            })
            .SingleAsync();

        return new DailyScheduleBookingOverview.Answer(
            result.Date,
            result.StartTime,
            result.EndTime,
            result.Courts,
            result.Bookings);
    }
}
