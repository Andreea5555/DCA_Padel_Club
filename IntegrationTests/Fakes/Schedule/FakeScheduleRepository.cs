
namespace IntegrationTests.Fakes.Schedule;

using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
public class FakeScheduleRepository: IScheduleRepository
{
    public List<Schedule> Schedules { get; } = new();
    public Task AddAsync(Schedule schedule)
    {
        Schedules.Add(schedule);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ScheduleId id)
    {
        throw new NotImplementedException();
    }

    public Task<Schedule?> GetAsync(ScheduleId id)
    {
        Schedule? foundSchedule= Schedules.FirstOrDefault(s => s.Id.Equals(id));
        return Task.FromResult(foundSchedule);
    }
}