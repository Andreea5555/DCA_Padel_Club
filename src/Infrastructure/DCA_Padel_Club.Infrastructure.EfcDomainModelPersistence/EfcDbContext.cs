using Microsoft.EntityFrameworkCore;

namespace DCA_Padel_Club.Infrastructure.EfcDomainModelPersistence;

public class EfcDbContext(DbContextOptions options): DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfcDbContext).Assembly);
    }
    
}