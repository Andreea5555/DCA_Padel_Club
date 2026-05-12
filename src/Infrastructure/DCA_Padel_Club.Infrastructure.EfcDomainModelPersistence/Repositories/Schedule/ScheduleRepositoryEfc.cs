using Microsoft.EntityFrameworkCore;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Repositories.Schedule;
using Core.Domain.Aggregates.Schedules;

public class ScheduleRepositoryEfc : RepositoryEfcBase<Schedule, ScheduleId>, IScheduleRepository
{
    internal IList<BookingPlayerReference> PlayerIds { get; private set; } = new List<BookingPlayerReference>();

    public ScheduleRepositoryEfc(EfcDbContext context) : base(context)
    {
    }

    public override async Task<Schedule?> GetAsync(ScheduleId id)
    {
        return await Context.Schedules
            .Include(x => x.Courts)
            .Include(x => x.Bookings)
            .SingleOrDefaultAsync(x => x.Id == id);
    }
}