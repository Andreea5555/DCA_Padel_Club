using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.ValueConverters;

public sealed class BookingIdConverter : ValueConverter<BookingId, Guid>
{
    public BookingIdConverter()
        : base(id => id.Id, value => new BookingId(value))
    {
    }
}
