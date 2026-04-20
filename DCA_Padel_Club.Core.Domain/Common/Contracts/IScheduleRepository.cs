using DCA_Padel_Club.Core.Domain.Common.Contracts;

namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public interface IScheduleRepository: IGenericRepository<Schedule, ScheduleId>
{
    Task AddAsync(Schedule schedule);
    Task<Schedule?> GetAsync(ScheduleId id);
}