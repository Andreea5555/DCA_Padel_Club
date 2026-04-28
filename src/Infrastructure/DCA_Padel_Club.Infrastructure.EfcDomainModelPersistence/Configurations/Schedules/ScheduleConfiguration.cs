using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.Configurations.Schedules;

public sealed class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.ToTable("Schedules");

        builder.HasKey(schedule => schedule.Id);

        builder.Property(schedule => schedule.Id)
            .ValueGeneratedNever();

        builder.Property(schedule => schedule.Date).IsRequired();
        builder.Property(schedule => schedule.StartTime).IsRequired();
        builder.Property(schedule => schedule.EndTime).IsRequired();
        builder.Property(schedule => schedule.IsDraft).IsRequired();
        builder.Property(schedule => schedule.IsDeleted).IsRequired();

        ConfigureCourts(builder);
        ConfigureBookingsRelationship(builder);
    }

    private static void ConfigureCourts(EntityTypeBuilder<Schedule> builder)
    {
        builder.OwnsMany(
            schedule => schedule.Courts,
            courtBuilder =>
            {
                courtBuilder.ToTable("ScheduleCourts");

                courtBuilder.WithOwner()
                    .HasForeignKey("ScheduleId");

                courtBuilder.Property<ScheduleId>("ScheduleId");

                courtBuilder.Property(court => court.Number)
                    .HasColumnName("CourtNumber")
                    .ValueGeneratedNever();

                courtBuilder.Property<bool>("isOccupied")
                    .HasField("isOccupied")
                    .UsePropertyAccessMode(PropertyAccessMode.Field)
                    .HasColumnName("IsOccupied")
                    .IsRequired();

                courtBuilder.HasKey("ScheduleId", nameof(PadelCourt.Number));
            });
    }

    private static void ConfigureBookingsRelationship(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasMany<Booking>(nameof(Schedule.Bookings))
            .WithOne()
            .HasForeignKey("ScheduleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(nameof(Schedule.Bookings))
            .HasField("_bookings")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
