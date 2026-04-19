using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Configurations.Schedules;

internal sealed class BookingPlayerReferenceConfiguration : IEntityTypeConfiguration<BookingPlayerReference>
{
    public void Configure(EntityTypeBuilder<BookingPlayerReference> builder)
    {
        builder.ToTable("BookingPlayers");

        builder.Property<BookingId>("BookingId");

        builder.Property(reference => reference.PlayerId)
            .HasColumnName("PlayerId")
            .IsRequired();

        builder.HasKey("BookingId", nameof(BookingPlayerReference.PlayerId));

        builder.HasOne<Player>()
            .WithMany()
            .HasForeignKey(reference => reference.PlayerId);
    }
}
