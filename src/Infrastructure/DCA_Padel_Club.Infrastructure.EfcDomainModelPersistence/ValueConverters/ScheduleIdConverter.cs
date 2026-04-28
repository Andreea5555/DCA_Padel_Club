using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.ValueConverters;

public sealed class ScheduleIdConverter : ValueConverter<ScheduleId, Guid>
{
    public ScheduleIdConverter()
        : base(id => id.Id, value => new ScheduleId(value))
    {
    }
}
