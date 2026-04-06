namespace DCA_Padel_Club.Core.Domain.Aggregates.Schedules;

public interface IScheduleRepository
{
    Task AddAsync(Schedule schedule);
    Task<Schedule?> GetAsync(ScheduleId id);
}