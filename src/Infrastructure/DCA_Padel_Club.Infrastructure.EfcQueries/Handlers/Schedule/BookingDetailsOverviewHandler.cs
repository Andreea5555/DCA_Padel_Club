using DCA_Padel_Club.Core.QueryContracts.Abstractions;
using DCA_Padel_Club.Core.QueryContracts.Queries;
using Microsoft.EntityFrameworkCore;

namespace DCA_Padel_Club.Infrastructure.EfcQueries.Handlers.Schedule;

public class BookingDetailsOverviewHandler(ViapadelClubContext context)
    : IQueryHandler<BookingDetailsOverview.Query, BookingDetailsOverview.Answer>
{
    public async Task<BookingDetailsOverview.Answer> HandleAsync(BookingDetailsOverview.Query query)
    {
        return await context.Bookings
            .Where(b => b.BookingId == query.BookingId)
            .Select(b => new BookingDetailsOverview.Answer(
                b.BookingId,
                b.ScheduleId,
                b.CourtNumber,
                b.SlotDate,
                b.SlotStartTime,
                b.SlotEndTime,
                b.BookerId
            ))
            .SingleAsync();
    }
}