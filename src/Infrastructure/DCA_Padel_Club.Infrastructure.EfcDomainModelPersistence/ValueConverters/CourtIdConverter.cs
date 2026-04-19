using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.ValueConverters;

public sealed class CourtIdConverter : ValueConverter<CourtId, string>
{
    public CourtIdConverter()
        : base(id => id.GetValue(), value => new CourtId(value))
    {
    }
}
