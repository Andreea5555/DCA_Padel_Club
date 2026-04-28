using DCA_Padel_Club.Core.Domain.Aggregates.Players;
using DCA_Padel_Club.Core.Domain.Aggregates.Schedules;
using DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence.ValueConverters;
using Microsoft.EntityFrameworkCore;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;

public class EfcDbContext(DbContextOptions<EfcDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Schedule> Schedules => Set<Schedule>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfcDbContext).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<ViaId>().HaveConversion<ViaIdConverter>();
        configurationBuilder.Properties<ScheduleId>().HaveConversion<ScheduleIdConverter>();
        configurationBuilder.Properties<BookingId>().HaveConversion<BookingIdConverter>();
        configurationBuilder.Properties<CourtId>().HaveConversion<CourtIdConverter>();
    }
}
