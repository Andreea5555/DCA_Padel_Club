using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.ValueConverters;

public sealed class ViaIdConverter : ValueConverter<ViaId, int>
{
    public ViaIdConverter()
        : base(id => id.Value, value => new ViaId(value))
    {
    }
}
