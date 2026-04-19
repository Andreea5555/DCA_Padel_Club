using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Configurations.Schedules;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(booking => booking.Id);

        builder.Property(booking => booking.Id)
            .HasColumnName("BookingId")
            .ValueGeneratedNever();

        builder.Property<ScheduleId>("ScheduleId");

        builder.Property<CourtId>("courtNumber")
            .HasField("courtNumber")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CourtNumber")
            .IsRequired();

        builder.Property<ViaId>("bookerId")
            .HasField("bookerId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("BookerId")
            .IsRequired();

        ConfigurePlayerIds(builder);
        ConfigureBookingSlot(builder);
    }

    private static void ConfigurePlayerIds(EntityTypeBuilder<Booking> builder)
    {
        builder.HasMany<BookingPlayerReference>("playerIds")
            .WithOne()
            .HasForeignKey("BookingId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("playerIds")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureBookingSlot(EntityTypeBuilder<Booking> builder)
    {
        builder.OwnsOne<BookingSlot>("bookingTimeSlot", slotBuilder =>
        {
            slotBuilder.Property(slot => slot.Date).HasColumnName("SlotDate").IsRequired();
            slotBuilder.Property(slot => slot.StartTime).HasColumnName("SlotStartTime").IsRequired();
            slotBuilder.Property(slot => slot.EndTime).HasColumnName("SlotEndTime").IsRequired();
        });

        builder.Navigation("bookingTimeSlot")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
